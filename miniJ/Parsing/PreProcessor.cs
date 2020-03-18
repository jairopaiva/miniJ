using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace miniJ.Parsing
{
    /// <summary>
    /// "Reconhece" o código, evitando assim que, primeiro tenha de se declarar os objetos e métodos para
    /// só depois os assinalemos. Faz também uma primeira análise sintática (apenas nas declarações mas não
    /// em seus corpos).
    /// </summary>
    class PreProcessor : ICompilerNode
    {
        public readonly TokenReader Reader;
        public Token curToken;

        private int _closedBlock;
        private int _openedBlocks;
        private AccessModifierNode _accessModifier;

        public PreProcessor()
        {
            Reader = new TokenReader(Helpers.Global.lexerTokenCollection);
            _openedBlocks = 0;
            _closedBlock = 0;
            _accessModifier = null;
        }

        public void Process(Namespace currentNamespace)
        {
            
            curToken = Reader.Peek();
            while (curToken.TokenType != TokenType.Delimiter_EOF)
            {
                if (curToken.TokenType == TokenType.Keyword_Namespace)
                {
                    ProcessNamespace(currentNamespace);
                }
                else if (curToken.IsToken(Token.ACCESS_MODIFIER))
                {
                    ProcessAccessModifier();
                }
                else if (ParserUtils.IsCISE(curToken))
                {
                    ProcessCISE(currentNamespace);
                }
                else if (curToken.TokenType == TokenType.Keyword_Using)
                {
                    ProcessUsing(currentNamespace);
                }
                else if (curToken.TokenType == TokenType.Delimiter_CBlock) // Namespace closing bracket
                {
                    NextToken();
                    Log(currentNamespace.ToString(), LogInfo.Created);
                }
                else
                {
                    throw new Exception(curToken.ToString());
                }
            }

          //  Log("Finish pre-processing, took: " + DateTime.Now.Subtract(started));
        }

        private void ProcessNamespace(Namespace currentNamespace)
        {
            NextToken(); // namespace

            Namespace curGlobal = currentNamespace.Clone();

            if (curToken.TokenType == TokenType.Delimiter_OBlock) // namespace com nome vazio
                goto namespaceGlobal;
            List<Token> namespaceName = ProcessDotExpr(TokenType.Delimiter_OBlock);

            foreach (Token nameToken in namespaceName)
            {
                if (curGlobal.Childs.Exists(n => n.Name.Value == nameToken.Value))
                {
                    curGlobal = curGlobal.Childs.Find(n => n.Name.Value == nameToken.Value);
                }
                else
                {
                    Namespace newN = new Namespace(nameToken, curGlobal);
                    if (curGlobal.Name == Helpers.Global.GlobalNamespace.Name)
                    {
                        Helpers.Global.GlobalNamespace.Childs.Add(newN);
                    }
                    else
                        curGlobal.Childs.Add(newN);
                    curGlobal = newN;
                }
            }

            Log(curGlobal.ToString(), LogInfo.Detected);

        verificaNamespaceVazio:

            NextToken(); // {
            if (curToken.TokenType == TokenType.Delimiter_CBlock) // Namespace vazio
            {
                Log("Cannot declare empthy namespaces!", LogInfo.Error);
            }

            Process(curGlobal);
            return;

        namespaceGlobal: // o namespace foi declarado sem nome, ou seja, tudo dentro dele fará parte de 'global'
            curGlobal = Helpers.Global.GlobalNamespace.Clone();
            goto verificaNamespaceVazio;
        }

        private void ProcessAccessModifier()
        {
            if (_accessModifier == null) // Esperado, caso contrário, algo deu errado previamente
            {
                _accessModifier = AccessModifierNode.Get(curToken);
                NextToken();
            }
            else
                Log("More than one access modifier for the same object.", LogInfo.Error);

            Log( _accessModifier.ToString(), LogInfo.Detected);

        }

        private void ProcessCISE(Namespace currentNamespace)
        {
            switch (curToken.TokenType)
            {
                case TokenType.Keyword_Class:
                    ProcessClass(currentNamespace);
                    break;

                case TokenType.Keyword_Interface:
                    ProcessInterface();
                    break;

                case TokenType.Keyword_Struct:
                    ProcessStruct();
                    break;

                case TokenType.Keyword_Enum:
                    ProcessEnum();
                    break;

                default:
                    throw new System.Exception();
            }
        }

        private void ProcessUsing(Namespace currentNamespace)
        {
            NextToken(); // using

            List<Token> usingPath = ProcessDotExpr(TokenType.Delimiter_CInstruction, true);
            string formattedUsingPath = string.Join(".", usingPath.Select(token => token.Value));

            if (!currentNamespace.CanImportNamespace(usingPath))
            {
                Log("Namespace cannot be found: " + formattedUsingPath, LogInfo.Error);
            }

            Log("Using at: " + currentNamespace.Name + " - Complete path: " +
               ParserUtils.GetDotExpr(usingPath), LogInfo.Detected);

            NextToken(); // ;
        }

        private void ProcessEnum()
        {
        }

        private void ProcessInterface()
        {
        }

        private void ProcessStruct()
        {
        }

        private void ProcessClass(Namespace currentNamespace)
        {
            NextToken(); // class
            Class Classe = new Class(curToken, curToken);
            Classe.AccessModifier = GetAccessModifier(SpecificAccessModifier.Private);
            NextToken();
            if (Expected_Token(false, TokenType.Delimiter_Collon))
            {
                while (curToken.TokenType != TokenType.Delimiter_OBlock)
                    NextToken();
            }

            currentNamespace.CISEs.Add(Classe.Name.Value, Classe);
            Classe.Namespace = currentNamespace;

            Log(Classe.ToString(), LogInfo.Detected);

            ProcessClassBody(Classe);

            Log(Classe.ToString(),  LogInfo.Created);
            NextToken(); // }
        }

        private void ProcessClassBody(Class fromClass)
        {
            Log(fromClass.Name.Value + " body.", LogInfo.Processing);
            Expected_Token(true, TokenType.Delimiter_OBlock);
            NextToken(); // {

            Token Const = null;

            while (curToken.TokenType != TokenType.Delimiter_CBlock)
            {
                if (curToken.IsToken(Token.ACCESS_MODIFIER))
                {
                    ProcessAccessModifier();
                }
                else if (curToken.IsToken(Token.PRIMITIVE_TYPE) || curToken.TokenType == TokenType.NotDef_TypeIdentifier)
                {
                    DataType type = ParseDataType();
                    Token name = curToken;
                    NextToken();

                    bool isConstant = Const != null;

                    switch (curToken.TokenType)
                    {
                        case TokenType.Delimiter_OParenthesis:
                            ParseMethod(name, type, fromClass);
                            break;
                        case TokenType.Operator_Equal: // Declarando uma variável
                            ParseVariableDeclaration(name, type, false, true, isConstant);
                            break;
                        case TokenType.Delimiter_CInstruction:
                            ParseVariableDeclaration(name, type, false, false, isConstant);
                            break;
                        default:
                            throw new Exception();
                    }
                }
                else if (curToken.TokenType == TokenType.Keyword_Constant)
                {
                    Const = curToken;
                    NextToken();
                }
                else
                {
                    throw new Exception();
                }
            }
            Log(fromClass.Name + " body.", LogInfo.Created);
        }

        private DataType ParseDataType()
        {
            DataType dataType;
            if (curToken.TokenType == TokenType.NotDef_TypeIdentifier)
            {
                dataType = new ObjectDataType(curToken)
                {
                    CISE = Helpers.Global.cisesDetectedInLexer.Find(cise => cise.Name.Value == curToken.Value)
                };
            }
            else if (curToken.IsToken(Token.PRIMITIVE_TYPE))
            {
                dataType = PrimitiveDataType.GetPrimitiveType(curToken);
            }
            else
                throw new Exception();

            NextToken();

            if (curToken.TokenType == TokenType.Delimiter_OIndex)
            {

                dataType.Settings = new DataType.DataTypeConfiguration() { Array = true };

                NextToken();
                Expected_Token(true, TokenType.Delimiter_CIndex);
                NextToken();
            }
            return dataType;
        }

        private void ParseMethod(Token name, DataType type, CISE cise)
        {
            Method method = new Method(name, type, GetAccessModifier(SpecificAccessModifier.Private));
            method.CISE = cise;
            method.Parameters = ParseMethodParametrsDclr();
            Expected_Token(true, TokenType.Delimiter_OBlock);
            NextToken();

            uint openBlocks = 1;

            while (openBlocks != 0)
            {
                if (curToken.TokenType == TokenType.Delimiter_OBlock)
                {
                    openBlocks++;
                }
                if (curToken.TokenType == TokenType.Delimiter_CBlock)
                {
                    openBlocks--;
                }
                NextToken();
            }

            Log(method.ToString(), LogInfo.Created);

        }

        private Variable ParseVariableDeclaration(Token Name, DataType dataType,
            bool insideMethod, bool Assigned, bool Constant)
        {
            Variable variable;

            if (!insideMethod) // Se estiver fora de um método, consiste em um Field
            {
                variable = new Field(dataType.Origin)
                {
                    AccessModifier = GetAccessModifier(SpecificAccessModifier.Private)
                };
            }
            else // Caso contrário, uma variável normal
            {
                variable = new Variable(dataType.Origin);
            }

            variable.Type = dataType;
            variable.Name = Name;
            variable.Constant = Constant;

            if (Assigned)
            {
                // variable.Value = ParseAsssignExpr();
                while (curToken.TokenType != TokenType.Delimiter_CInstruction)
                {
                    NextToken();
                }
                NextToken();
            }
            else
            {
                if(Constant) // Uma constante necessita ser assinalada ao ser declarada
                {
                    Log("A constant needs to be flagged when it is declared.", LogInfo.Error);
                }

                Expected_Token(true, TokenType.Delimiter_CInstruction);
                NextToken();
            }

            Log(variable.ToString(), LogInfo.Created);

            return variable;
        }

        private List<ParameterDeclaration> ParseMethodParametrsDclr(Method curMethod = null)
        {
            NextToken(); // (
            List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();

            while (curToken.TokenType != TokenType.Delimiter_CParenthesis)
            {
                parameters.Add(ParseParameterDeclaration());
            }

            NextToken();

            return parameters;
        }

        private ParameterDeclaration ParseParameterDeclaration()
        {
            if (!Expected_Token(false, TokenType.NotDef_TypeIdentifier) && !curToken.IsToken(Token.PRIMITIVE_TYPE))
            {
                throw new Exception();
            }

            DataType dataType = ParseDataType();

            ParameterDeclaration parameter = new ParameterDeclaration(curToken);
            parameter.Type = dataType;
            parameter.Name = curToken;

            NextToken();

            switch (curToken.TokenType)
            {
                case TokenType.Operator_Equal:
                    parameter.Value = ParseAsssignExpr();
                    break;
                case TokenType.Delimiter_Comma:
                    NextToken();
                    break;
                case TokenType.Delimiter_CParenthesis:
                    break;
                default:
                    throw new Exception();
            }

            return parameter; ;
        }

        private Expression ParseAsssignExpr()
        {
            Expected_Token(true, TokenType.Operator_Equal);
            NextToken(); // =
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retonar o modificador de accesso que provavelmente, já foi achado, caso não tenha sido declarado
        /// de forma explicita, a função retorna o modificador de acesso padrão para o tipo de objeto.
        /// </summary>
        /// <param name="defaultAccessModifier">Modificador de acesso padrão caso não o tenha</param>
        /// <returns>Modificador de acesso</returns>
        public AccessModifierNode GetAccessModifier(SpecificAccessModifier defaultAccessModifier)
        {
            if (_accessModifier != null)
            {
                AccessModifierNode result = _accessModifier;
                _accessModifier = null;
                return result;
            }
            return AccessModifierNode.Get(curToken, defaultAccessModifier);
        }

        private void NextToken()
        {
            Reader.Read();
            curToken = Reader.Peek();
            switch (curToken.TokenType)
            {
                case TokenType.Delimiter_OBlock:
                    _openedBlocks++;
                    break;

                case TokenType.Delimiter_CBlock:
                    _closedBlock++;
                    break;
            }
        }

        private List<Token> ProcessDotExpr(TokenType delimiter, bool Identifier = false)
        {
            List<Token> result = new List<Token>();
            while (curToken.TokenType != delimiter)
            {
                if (curToken.TokenType != TokenType.Delimiter_Dot)
                {
                    if (Identifier && !ParserUtils.ValidIdentifier(curToken.Value))
                    {
                        Log("Invalid identifier!", LogInfo.Error);
                    }
                    result.Add(curToken);
                }
                NextToken();
            }
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="expectedToken">Token esperado</param>
        /// <param name="throwable">Se verdadeiro, ele cria uma exceção ao não achar o token esperado</param>
        /// <returns>Satisfez a condição, encontrou o token esperado?</returns>
        private bool Expected_Token(bool throwable, params TokenType[] expectedTokens)
        {
            bool result = false;

            for (int i = 0; i < expectedTokens.Length; i++)
            {
                if (expectedTokens[i] == curToken.TokenType)
                {
                    result = true;
                    break;
                }
            }

            if (!result && throwable)
                Log("Expected " + ParserUtils.GetExpectedTokenListAsString(expectedTokens) , LogInfo.Error);

            return result;
        }

        private void Debug(bool readKey = true, string text = "", Token token = null)
        {
            if (token != null)
                Console.WriteLine(text + token.ToString());
            else
                Console.WriteLine(text + curToken.ToString());
            if (readKey)
                Console.ReadKey();
        }

        private void Log(string message, LogInfo typeOfLog)
        {
            string msg = string.Empty;
            bool error = false;
            
            switch (typeOfLog)
            {
                case LogInfo.Created:
                    msg = "Created: ";
                    break;
                case LogInfo.Processing:
                    msg = "Processing: ";
                    break;
                case LogInfo.Detected:
                    msg = "Detected: ";
                    break;
                case LogInfo.Error:
                    error = true;
                    msg = "Error: ";
                    break;
                case LogInfo.Info:
                    error = true;
                    msg = "Info: ";
                    break;
                default:
                    break;
            }
            

            msg += message;
            Helpers.Global.Logger.Log(msg, this);

            if (error)
                throw new Exception(msg + Environment.NewLine + curToken.ToString());
        }

        private enum LogInfo
        {
            Created,
            Processing,
            Detected,
            Error,
            Info
        }

    }
}
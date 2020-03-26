using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using miniJ.Parsing.Elements.DataTypes;
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
        private AccessModifierNode _accessModifier;
        private Namespace _globalNamespace;
        private LexerResult lexerResult;
        private TokenReader reader;
        public Token curToken;

        private List<CodeError> detectedErrors;
        private int _openedBlocks;
        private int _closedBlock;
        public PreProcessor()
        {
            _accessModifier = null;
            _openedBlocks = 0;
            _closedBlock = 0;
        }

        public List<CodeError> Start(LexerResult LexerResult, Namespace globalNamespace)
        {
            detectedErrors = new List<CodeError>();
            _globalNamespace = globalNamespace;
            lexerResult = LexerResult;
            reader = new TokenReader(lexerResult.lexerTokenCollection);
            ProcessNamespaceBody(_globalNamespace);
            return detectedErrors;
        }

        private void ProcessNamespaceBody(Namespace currentNamespace)
        {
            curToken = reader.Peek();
            while (curToken.TokenType != TokenType.Delimiter_EOF)
            {
                if (curToken.TokenType == TokenType.Keyword_Namespace)
                {
                    ProcessNamespaceDeclaration(currentNamespace);
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
                else if (curToken.IsToken(Token.ACCESS_MODIFIER))
                {
                    ProcessAccessModifier(false);
                }
                else if (ParserUtils.IsCISE(curToken))
                {
                    ProcessCISE(currentNamespace);
                }
                else
                {
                    throw new Exception(curToken.ToString());
                }
            }
        }

        private void ProcessNamespaceDeclaration(Namespace currentNamespace)
        {
            Token origin = curToken;
            NextToken(); // namespace
            Namespace curGlobal = currentNamespace.Clone();

            if (curToken.TokenType == TokenType.Delimiter_OBlock) // namespace com nome vazio
            {
                Error("Expected namespace path(name)!", curToken);
            }

            List<Token> namespaceName = ProcessDotExpr(TokenType.Delimiter_OBlock);

            foreach (Token nameToken in namespaceName)
            {
                if (curGlobal.Childs.Exists(n => n.Name == nameToken.Value))
                {
                    curGlobal = curGlobal.Childs.Find(n => n.Name == nameToken.Value);
                }
                else
                {
                    Namespace newN = new Namespace(origin, curGlobal);
                    newN.Name = nameToken.Value;

                    if (curGlobal.Name == _globalNamespace.Name)
                    {
                        _globalNamespace.Childs.Add(newN);
                    }
                    else
                    {
                        curGlobal.Childs.Add(newN);
                    }

                    curGlobal = newN;
                }
            }

            Log(curGlobal.ToString(), LogInfo.Detected);

            NextToken(); // {
            if (curToken.TokenType == TokenType.Delimiter_CBlock) // Namespace vazio
            {
                Error("Cannot declare empthy namespaces!", curToken);
            }

            ProcessNamespaceBody(curGlobal);
        }

        private void ProcessAccessModifier(bool insideCISE)
        {
            if (_accessModifier == null) // Esperado, caso contrário, algo deu errado previamente
            {
                _accessModifier = AccessModifierNode.Get(curToken);
                NextToken();

                Log(_accessModifier.ToString(), LogInfo.Detected);
            }
            else
            {
                Error("More than one access modifier for the same object.", curToken);
            }
        }

        private void ProcessCISE(Namespace currentNamespace, CISE cise = null)
        {
            switch (curToken.TokenType)
            {
                case TokenType.Keyword_Class:
                    ProcessClass(currentNamespace, cise);
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
                    throw new Exception();
            }
        }

        private void ProcessUsing(Namespace currentNamespace)
        {
            NextToken(); // using

            List<Token> usingPath = ProcessDotExpr(TokenType.Delimiter_CInstruction, true);
            string formattedUsingPath = string.Join(".", usingPath.Select(token => token.Value));

            if (!currentNamespace.CanImportNamespace(usingPath))
            {
                Log("Using at: " + currentNamespace.Name + " - Complete path: " +
                    ParserUtils.GetDotExpr(usingPath), LogInfo.Detected);
                NextToken(); // ;
            }
            else
            {
                Error("Namespace cannot be found: " + formattedUsingPath + "!", usingPath[0]);
            }
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

        /// <summary>
        /// Processa a declaração de uma classe.
        /// </summary>
        /// <param name="currentNamespace">Namespace atual em que se encontra a declaração.</param>
        /// <param name="cise">Quando este parâmetro é diferente de null, quer dizer que a classe a ser processada foi declarada dentro de um CISE</param>
        private void ProcessClass(Namespace currentNamespace, CISE cise = null)
        {
            NextToken(); // class
            Class Classe = new Class(curToken, curToken);
            Classe.AccessModifier = GetAccessModifier(SpecificAccessModifier.PRIVATE);
            Classe.Namespace = currentNamespace;

            NextToken();
            if (Expected_Token(false, TokenType.Delimiter_Collon))
            {
                while (curToken.TokenType != TokenType.Delimiter_OBlock)
                {
                    NextToken();
                }
            }

            if (cise == null) // Esta sendo declarado "fora" de um outro CISE (dentro apenas de um namespace ou global mesmo)
            {
                currentNamespace.CISEs.Add(Classe.Name, Classe);
                Log(Classe.ToString(), LogInfo.Detected);
            }
            else             // Esta sendo declarado "dentro" de um outro CISE
            {
                Classe.Root = cise;
                cise.Children.Add(Classe);
                Log(Classe.ToString(), LogInfo.Detected);
            }

            ProcessClassBody(Classe, cise);

            Log(Classe.ToString(), LogInfo.Created);
            NextToken(); // }
        }

        private void ProcessClassBody(Class toClass, CISE fromCISE = null)
        {
            Log(toClass.Name + " body.", LogInfo.Processing);
            Expected_Token(true, TokenType.Delimiter_OBlock);
            NextToken(); // {

            while (curToken.TokenType != TokenType.Delimiter_CBlock)
            {
                if (curToken.IsToken(Token.ACCESS_MODIFIER))
                {
                    ProcessAccessModifier(true);
                }
                else if (ParserUtils.IsTypeOrModifierRelatedToType(curToken)) // Um DataType ou um modificador relacionado
                {
                    DataType type = ParseDataType();

                    if (VerifyErrorOcurred(type, typeof(DataType)))
                    {
                        continue;
                    }

                    List<Token> names = new List<Token>();
                    bool openComma = false;
                    while(curToken.TokenType == TokenType.NotDef_Identifier || curToken.TokenType == TokenType.Delimiter_Comma)
                    {
                        if(curToken.TokenType == TokenType.NotDef_Identifier)
                        {
                            names.Add(curToken);
                            openComma = false;
                        }
                        else if(curToken.TokenType == TokenType.Delimiter_Comma)
                        {
                            openComma = true;
                        }
                        else
                        {
                            throw new Exception(curToken.ToString());
                        }

                        NextToken();
                    }

                    if (openComma)
                    {
                        Error("Expected variable name.", curToken);
                    }

                    switch (curToken.TokenType)
                    {
                        case TokenType.Delimiter_OParenthesis:
                            {
                                if (names.Count > 1)
                                {
                                    Error("Invalid method name.", names[1]);
                                }

                                ParseMethod(names[0], type, toClass);
                                break;
                            }
                        case TokenType.Operator_Equal: // Declarando uma variável
                        case TokenType.Delimiter_CInstruction:
                            {
                                ParseVariableDeclaration(names, type, false);
                                break;
                            }
                        default:
                            throw new Exception(curToken.ToString());
                    }
                }
                else if (ParserUtils.IsCISE(curToken))
                {
                    ProcessCISE(toClass.Namespace, toClass);
                }
                else
                {
                    throw new Exception(curToken.ToString());
                }
            }

            Log(toClass.Name + " body.", LogInfo.Created);
        }

        private DataType ParseDataType()
        {
            DataType dataType;

            byte dataTypeAssignedSpecs = 0; // Conta quanto dessas variáveis bool abaixo foram assinaladas como true
            bool Constant = false; // <-- Por padrão, todos os objetos são variáveis
            bool ReadOnly = false; 
            bool Volatile = false; 
            bool Static = false;

            int firstModifierIndex = reader.Position;

            while (!ParserUtils.IsType(curToken))
            {
                switch (curToken.TokenType)
                {
                    case TokenType.Keyword_Constant:
                        Constant = true;
                        dataTypeAssignedSpecs++;
                        NextToken();
                        break;
                    case TokenType.Keyword_Readonly:
                        ReadOnly = true;
                        dataTypeAssignedSpecs++;
                        NextToken();
                        break;
                    case TokenType.Keyword_Volatile:
                        Volatile = true;
                        dataTypeAssignedSpecs++;
                        NextToken();
                        break;
                    case TokenType.Keyword_Static:
                        Static = true;
                        dataTypeAssignedSpecs++;
                        NextToken();
                        break;
                    default:
                        throw new Exception(curToken.ToString());
                }
            }

            if (dataTypeAssignedSpecs > 1)
            {
                if (!Static)
                {
                    Error("You can only have one signature modifier per type! Modifiers: "
                        + ParserUtils.GetSignatureModifiers(Constant, ReadOnly, Volatile, Static) + ".", reader[firstModifierIndex + 1]);
                    return null;
                }
            }

            if (curToken.TokenType == TokenType.NotDef_TypeIdentifier)
            {
                dataType = new ObjectDataType(curToken)
                {
                    CISE = lexerResult.cisesDetectedInLexer.Find(cise => cise.Name == curToken.Value)
                };
            }
            else 
            {
                dataType = PrimitiveDataType.GetPrimitiveType(curToken);
            }

            NextToken();
            bool Array = false;
            
            if (curToken.TokenType == TokenType.Delimiter_OIndex)
            {
                Array = true ;

                NextToken();
                Expected_Token(true, TokenType.Delimiter_CIndex);
                NextToken();
            }

            dataType.Settings = new DataTypeConfiguration()
            {
                Array = Array,
                Constant = Constant,
                ReadOnly = ReadOnly,
                Volatile = Volatile
            };

            return dataType;
        }

        private void ParseMethod(Token name, DataType type, CISE cise)
        {
            AbstractMethod method = new AbstractMethod(name, type, GetAccessModifier(SpecificAccessModifier.PRIVATE));

            method.CISE = cise;
            method.Parameters = ParseMethodParametrsDclr();

            if (cise.TypeOfCISE != CISE.SpecificTypeOfCISE.Interface)
            {

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

            }
            else
            {
                Expected_Token(true, TokenType.Delimiter_Collon);
                NextToken();
            }

            Log(method.ToString(), LogInfo.Created);

        }

        private List<Variable> ParseVariableDeclaration(List<Token> Names, DataType dataType,bool insideMethod)
        {
            List<Variable> dclrVariables = new List<Variable>();

            for (int nameIndex = 0; nameIndex < Names.Count; nameIndex++)
            {
                Variable variable;
                if (!insideMethod) // Se estiver fora de um método, consiste em um Field
                {
                    variable = new Field(Names[nameIndex])
                    {
                        AccessModifier = GetAccessModifier(SpecificAccessModifier.PRIVATE)
                    };
                }
                else // Caso contrário, uma variável normal
                {
                    variable = new Variable(Names[nameIndex]);
                }

                variable.Type = dataType;
                variable.Name = Names[nameIndex].Value;

                dclrVariables.Add(variable);
            }

            if (curToken.TokenType == TokenType.Operator_Equal)
            {
                //  Expression variablesValue = ParseAsssignExpr();
                while (curToken.TokenType != TokenType.Delimiter_CInstruction)
                {
                    NextToken();
                }
                NextToken(); // ;
            }
            else
            {
                if (dataType.Settings.Constant) // Uma constante necessita ser assinalada ao ser declarada
                {
                    Error("A constant needs to be flagged when it is declared.", curToken);
                }

                Expected_Token(true, TokenType.Delimiter_CInstruction);
                NextToken();
            }

            foreach (Variable variable in dclrVariables)
            {
                Log(variable.ToString(), LogInfo.Created);
            }
            return dclrVariables;
        }

        private List<ParameterDeclaration> ParseMethodParametrsDclr(AbstractMethod curMethod = null)
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
            if (!Expected_Token(false, TokenType.NotDef_TypeIdentifier) && !curToken.IsToken(Token.BuiltInType))
            {
                throw new Exception(curToken.ToString());
            }

            DataType dataType = ParseDataType();

            ParameterDeclaration parameter = new ParameterDeclaration(dataType.Origin);
            parameter.Type = dataType;
            parameter.Name = curToken.Value;

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
                    throw new Exception(curToken.ToString());
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
            reader.Read();
            curToken = reader.Peek();
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
                        Error("Invalid identifier!", curToken);
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
            {
                Error("Expected " + ParserUtils.GetExpectedTokenListAsString(expectedTokens), curToken);
            }

            return result;
        }

        private void Debug(bool readKey = true, string text = "")
        {
            Console.WriteLine(text + curToken.ToString());

            if (readKey)
            {
                Console.ReadKey();
            }
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
        }

        /// <summary>
        ///  Desta forma, fazemos com que seja possível descobrir o máximo de erros numa única verificação
        /// </summary>
        /// <param name="message">Especificação do erro</param>
        /// <param name="errorToken">Token que causa o erro</param>
        private void Error(string message, Token errorToken)
        {
            Log(message, LogInfo.Error);

            while (curToken.Location.File == errorToken.Location.File
                  && curToken.Location.Line == errorToken.Location.Line)
            {
                NextToken();
            }

            //  Debug(true, "Final do erro");
            CodeError codeError = new CodeError(errorToken, this, message);
            detectedErrors.Add(codeError);
        }

        private bool VerifyErrorOcurred(ISyntaxNode returnedNode, Type expectedResultType)
        {
            bool errorOcurred = returnedNode == null;
            if(expectedResultType == typeof(DataType))
            {
                _accessModifier = null;
            }

            return errorOcurred;
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
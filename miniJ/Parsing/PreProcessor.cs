using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Elements.Base.CompilationElements;
using miniJ.Elements.Base.Error;
using miniJ.Helpers;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using miniJ.Parsing.Elements.DataTypes;
using miniJ.Parsing.Elements.Symbols;
using System;
using System.Collections.Generic;

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
        private CompilationUnit _compilationUnit;
        private TokenReader reader;
        private Token curToken;

        private readonly List<Token> _bracketsInCode;
        private int _openedBlocks;
        private int _closedBlock;

        public PreProcessor()
        {
            _bracketsInCode = new List<Token>();
            _accessModifier = null;
            _openedBlocks = 0;
            _closedBlock = 0;
        }

        public void Start(ref CompilationUnit compilationUnit)
        {
            compilationUnit.LexerResult.SetAllTypeIdentifiers();
            _compilationUnit = compilationUnit;
            reader = new TokenReader(_compilationUnit.LexerResult.Tokens);
            ProcessNamespaceBody(_compilationUnit.GlobalNamespace);
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
                    if (currentNamespace.Open)
                    {
                        if (currentNamespace.Name != _compilationUnit.GlobalNamespace.Name)
                        {
                            currentNamespace.Open = false;
                            Log(currentNamespace.ToString(), LogInfo.Created);
                            NextToken();
                            return;
                        }
                        else
                        {
                            Error(ErrorMessages.INVALID_TOKEN, curToken, ErrorLocation.NamespaceBody, currentNamespace);
                        }
                    }
                    else
                    {
                        int lastBeforeThis = _bracketsInCode.Count - 2;
                        Error(ErrorMessages.NAMESPACE_ALREADY_CLOSED + _bracketsInCode[lastBeforeThis].Location.ToString()
                            , curToken, ErrorLocation.NamespaceBody, currentNamespace);
                    }
                }
                else if (curToken.IsToken(Token.ACCESS_MODIFIER))
                {
                    ProcessAccessModifier(currentNamespace);
                }
                else if (ParserUtils.IsCISE(curToken))
                {
                    ProcessCISE(currentNamespace);
                }
                else
                {
                    Error("Expected namespace, modifier, type declaration or EOF.", curToken, ErrorLocation.NamespaceBody
                         , currentNamespace, false);
                    NextToken();
                }
            }
        }

        private void ProcessNamespaceDeclaration(Namespace currentNamespace)
        {
            Token origin = curToken;
            NextToken(); // namespace
            Namespace curGlobal = currentNamespace.Clone();

            if (!Expected_Token(TokenType.NotDef_Identifier, ErrorLocation.NamespaceBody, currentNamespace))
            {
                return; // Algum erro ocorreu, então retormos a estaca 0
            }

            List<Token> namespaceName = ProcessNamespaceName(TokenType.Delimiter_OBlock);

            if (!Expected_Token(TokenType.Delimiter_OBlock, ErrorLocation.NamespaceBody, currentNamespace))
            {
                return; // Algum erro ocorreu, então retormos a estaca 0
            }

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

                    if (curGlobal.Name == _compilationUnit.GlobalNamespace.Name)
                    {
                        _compilationUnit.GlobalNamespace.Childs.Add(newN);
                    }
                    else
                    {
                        curGlobal.Childs.Add(newN);
                    }

                    curGlobal = newN;
                }
            }

            Log(curGlobal.ToString(), LogInfo.Detected);

            Expected_Token(TokenType.Delimiter_OBlock, ErrorLocation.NamespaceDclr, curGlobal);

            NextToken(); // {
            if (curToken.TokenType == TokenType.Delimiter_CBlock) // Namespace vazio
            {
                Error("Cannot declare empthy namespaces!", curToken, ErrorLocation.NamespaceDclr, curGlobal);
            }


            ProcessNamespaceBody(curGlobal);
        }

        private void ProcessAccessModifier(SyntaxNode calledFromNode)
        {
            if (_accessModifier == null) // Esperado, caso contrário, algo deu errado previamente
            {
                _accessModifier = AccessModifierNode.Get(curToken);
                NextToken();

                Log(_accessModifier.ToString(), LogInfo.Detected);
            }
            else
            {
                _accessModifier = null;
                Debug(true, "tey:");
                Error("More than one access modifier for the same object.", curToken, ErrorLocation.AccessModifier, calledFromNode);
                Debug(true, "blalala:");
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
            throw new NotImplementedException();
            /*
            NextToken(); // using

            List<Token> usingPath = ProcessNamespaceName(TokenType.Delimiter_CInstruction, true);
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
            */
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
            Token origin = curToken;
            NextToken(); // class

            if (!Expected_Token(TokenType.NotDef_TypeIdentifier, ErrorLocation.CISEsDclr, currentNamespace))
            {
                _accessModifier = null;
                return;
            }

            Class Classe = new Class(curToken, origin);
            Classe.AccessModifier = GetAccessModifier(SpecificAccessModifier.PRIVATE);
            Classe.Namespace = currentNamespace;

            NextToken();
            if (curToken.TokenType == TokenType.Delimiter_Collon)
            {
                while (curToken.TokenType != TokenType.Delimiter_OBlock)
                {
                    NextToken();
                }
            }

            if (cise == null) // Esta sendo declarado dentro apenas de um namespace ou global mesmo
            {
                if (!currentNamespace.CISEs.ContainsKey(Classe.Name))
                {
                    currentNamespace.CISEs.Add(Classe.Name, Classe);
                    Log(Classe.ToString(), LogInfo.Detected);
                }
                else // Erro, pois já existe uma classe com mesmo nome dentro do namespace
                {
                    Error("Already exists an class with same name inside the namespace '" + currentNamespace.Name + "'.", curToken, ErrorLocation.CISEsDclr, Classe);
                    return;
                }
            }
            else             // Esta sendo declarado "dentro" de um outro CISE
            {
                Classe.Root = cise;
                if (!cise.Children.Exists(c => c.Name == Classe.Name))
                {
                    cise.Children.Add(Classe);
                    Log(Classe.ToString(), LogInfo.Detected);
                }
                else // Erro, pois já existe uma classe com mesmo nome dentro da classe raiz
                {
                    Error("Already exists an class with same name inside the class '" + cise.Name + "'.", curToken, ErrorLocation.CISEsDclr, Classe);
                    return;
                }
            }

           if(ProcessClassBody(Classe))
            {
                NextToken(); // }
            }
            else
            {
               // Debug();
            }

            Log(Classe.ToString(), LogInfo.Created);
        }

        private bool ProcessClassBody(Class toClass)
        {
            Log(toClass.Name + " body.", LogInfo.Processing);
            Expected_Token(TokenType.Delimiter_OBlock, ErrorLocation.CISEsBody, toClass, false);
            NextToken(); // {

            bool noErrors = true;

            while (curToken.TokenType != TokenType.Delimiter_CBlock)
            {
                if (curToken.IsToken(Token.ACCESS_MODIFIER))
                {
                    ProcessAccessModifier(toClass);
                }
                else if (ParserUtils.IsTypeOrModifierRelatedToType(curToken, _compilationUnit)) // Um DataType ou um modificador relacionado
                {
                    bool Virtual = false;
                    DataType type = ParseDataType(out Virtual, toClass);

                    if (type == null) // Ocorreu algum erro
                    {
                        _accessModifier = null;
                        continue;
                    }

                    if (curToken.TokenType == TokenType.Delimiter_OParenthesis) // Construtor
                    {
                        toClass.Constructor = ParseMethod(ParserUtils.CONSTRUCTOR_NAME, type, toClass, Virtual);
                        continue;
                    }

                    List<Token> names = new List<Token>();
                    bool openComma = false;
                    while (curToken.TokenType == TokenType.NotDef_Identifier || curToken.TokenType == TokenType.Delimiter_Comma)
                    {
                        if (curToken.TokenType == TokenType.NotDef_Identifier)
                        {
                            names.Add(curToken);
                            openComma = false;
                        }
                        else if (curToken.TokenType == TokenType.Delimiter_Comma)
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
                        Error("Expected variable name.", curToken, ErrorLocation.CISEsBody, toClass);
                    }

                    switch (curToken.TokenType)
                    {
                        case TokenType.Delimiter_OParenthesis:
                            {
                                if (names.Count > 1)
                                {
                                    Error("Invalid method name.", names[1], ErrorLocation.CISEsBody, toClass);
                                }

                                toClass.Methods.Add(ParseMethod(names[0].Value, type, toClass, Virtual));
                                break;
                            }
                        case TokenType.Operator_Equal: // Declarando uma variável
                        case TokenType.Delimiter_CInstruction:
                            {
                                if (Virtual)
                                {
                                    Error(ErrorMessages.INVALID_MEMBER_MODIFIER, names[0], ErrorLocation.CISEsBody, toClass);
                                }

                                toClass.Fields.AddRange(Field.Convert(ParseVariableDeclaration(names, type, toClass), GetAccessModifier(SpecificAccessModifier.PRIVATE)));
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
                    bool panic = curToken.TokenType == TokenType.Keyword_Namespace;
                    Expected_Token(TokenType.Delimiter_CBlock, ErrorLocation.CISEsBody, toClass, panic);
                    noErrors = false;

                    if (panic)
                    {
                        break;
                    }
                }
            }

            Log(toClass.Name + " body.", LogInfo.Created);
            return noErrors;
        }
        private DataType ParseDataType(out bool Virtual, SyntaxNode calledFromNode)
        {
            DataType dataType;

            byte dataTypeAssignedSpecs = 0; // Conta quanto dessas variáveis bool abaixo foram assinaladas como true
            bool Constant = false; // <-- Por padrão, todos os objetos são variáveis
            bool ReadOnly = false;
            bool Volatile = false;
            bool Static = false;
            Virtual = false;

            int firstModifierIndex = reader.Position;

            while (!ParserUtils.IsType(curToken, _compilationUnit.LexerResult.CISES))
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
                    case TokenType.Keyword_Virtual:
                        Virtual = true;
                        dataTypeAssignedSpecs++;
                        NextToken();
                        break;
                    default:
                        throw new Exception(curToken.ToString());
                }
            }

            if (dataTypeAssignedSpecs > 1 && (!Static || Virtual))
            {
                Error("You can only have one signature modifier per type! Modifiers: "
                    + ParserUtils.GetSignatureModifiers(Constant, ReadOnly, Volatile, Static, Virtual) + ".", reader[firstModifierIndex + 1]
                    , ErrorLocation.DataType, calledFromNode);
                return null;
            }

            if (curToken.TokenType == TokenType.NotDef_TypeIdentifier)
            {
                dataType = new ObjectDataType(curToken)
                {
                    // CISE = lexerResult.CISES.Find(cise => cise.Name == curToken.Value)
                };
            }
            else
            {
                dataType = PrimitiveDataType.GetPrimitiveType(curToken);
            }

            NextToken();
            bool Array = false;

            if (Expected_Token(UnionTokenTypes(TokenType.NotDef_Identifier, TokenType.Delimiter_OIndex, TokenType.Delimiter_OParenthesis), ErrorLocation.DataType, calledFromNode))
            {
                if (curToken.TokenType == TokenType.Delimiter_OIndex)
                {
                    Array = true;
                    NextToken();

                    if (Expected_Token(TokenType.Delimiter_CIndex, ErrorLocation.DataType, calledFromNode))
                    {
                        NextToken();
                    }
                }
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

        private Method ParseMethod(string name, DataType type, CISE cise, bool Virtual)
        {
            Method method = new Method(name, type, GetAccessModifier(SpecificAccessModifier.PRIVATE));

            method.CISE = cise;
            method.Parameters = ParseMethodParametrsDclr(method);
            method.Virtual = Virtual;
          
            if (name == ParserUtils.CONSTRUCTOR_NAME)
            {
                if (Virtual)
                {
                    Error(ErrorMessages.INVALID_MEMBER_MODIFIER, type.Origin, ErrorLocation.MethodDclr, method);
                }
                cise.Constructor = method;
            }

            if (cise.TypeOfCISE != CISE.SpecificTypeOfCISE.Interface)
            {

                Expected_Token(TokenType.Delimiter_OBlock, ErrorLocation.MethodDclr, method);
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
                //Error("Interface methods cannot have a body assigned.", curToken, ErrorLocation.MethodDclr, method, false);
                Expected_Token(TokenType.Delimiter_CInstruction, ErrorLocation.MethodDclr, method);
                NextToken();
            }

            Log(method.ToString(), LogInfo.Created);
            return method;
        }

        private List<Variable> ParseVariableDeclaration(List<Token> Names, DataType dataType, SyntaxNode calledFromNode)
        {
            List<Variable> dclrVariables = new List<Variable>();

            for (int nameIndex = 0; nameIndex < Names.Count; nameIndex++)
            {
                Variable variable = new Variable(Names[nameIndex]);
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
                    Error("A constant needs to be flagged when it is declared.", curToken
                          , ErrorLocation.VariableBody, calledFromNode);
                }

                Expected_Token(TokenType.Delimiter_CInstruction, ErrorLocation.VariableDclr, calledFromNode);
                NextToken();
            }

            foreach (Variable variable in dclrVariables)
            {
                Log(variable.ToString(), LogInfo.Created);
            }

            return dclrVariables;
        }

        private List<ParameterDeclaration> ParseMethodParametrsDclr(SyntaxNode calledFromNode)
        {
            NextToken(); // (

            List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();

            if (curToken.TokenType == TokenType.Delimiter_CParenthesis) // Sem parâmetros
            {
                NextToken();
                return parameters;
            }

            while (curToken.TokenType != TokenType.Delimiter_CParenthesis)
            {
                parameters.Add(ParseParameterDeclaration(calledFromNode));
            }

            NextToken();

            return parameters;
        }

        private ParameterDeclaration ParseParameterDeclaration(SyntaxNode calledFromNode)
        {
            if (!Expected_Token(UnionTokenTypes(TokenType.NotDef_TypeIdentifier, ParserUtils.AllBuiltInTokens), ErrorLocation.ParameterDclr, calledFromNode))
            {
                if (curToken.TokenType == TokenType.Delimiter_Comma)
                {
                    NextToken();
                }

                return null;
            }

            Token error = curToken;
            bool Virtual = false;
            DataType dataType = ParseDataType(out Virtual, calledFromNode);

            ParameterDeclaration parameter = new ParameterDeclaration(dataType.Origin);
            parameter.Type = dataType;
            parameter.Name = curToken.Value;

            if (Virtual)
            {
                Error(ErrorMessages.INVALID_MEMBER_MODIFIER, error, ErrorLocation.ParameterDclr, parameter);
                return null;
            }

            NextToken();

            switch (curToken.TokenType)
            {
                case TokenType.Operator_Equal:
                    parameter.Value = ParseAsssignExpr(parameter);
                    break;
                case TokenType.Delimiter_Comma:
                    NextToken();
                    break;
                case TokenType.Delimiter_CParenthesis:
                    break;
                default:
                    throw new Exception(curToken.ToString());
            }

            Log(parameter.ToString(), LogInfo.Created);
            return parameter;
        }

        private Expression ParseAsssignExpr(SyntaxNode calledFromNode)
        {
            Expected_Token(TokenType.Operator_Equal, ErrorLocation.AssignExpr, calledFromNode);
            NextToken(); // =
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retonar o modificador de accesso que provavelmente, já foi achado, caso não tenha sido declarado
        /// de forma explicita, a função retorna o modificador de acesso padrão para o tipo de objeto.
        /// </summary>
        /// <param name="defaultAccessModifier">Modificador de acesso padrão caso não o tenha</param>
        /// <returns>Modificador de acesso</returns>
        private AccessModifierNode GetAccessModifier(SpecificAccessModifier defaultAccessModifier)
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

            if (curToken.TokenType == TokenType.Delimiter_OBlock || curToken.TokenType == TokenType.Delimiter_CBlock)
            {
                switch (curToken.TokenType)
                {
                    case TokenType.Delimiter_OBlock:
                        _openedBlocks++;
                        break;
                    case TokenType.Delimiter_CBlock:
                        _closedBlock++;
                        break;
                }
                _bracketsInCode.Add(curToken);
            }
        }

        private List<Token> ProcessNamespaceName(TokenType delimiter)
        {
            List<Token> result = new List<Token>();
            while (curToken.TokenType != delimiter)
            {
                if (curToken.TokenType == TokenType.NotDef_Identifier)
                {
                    result.Add(curToken);
                    NextToken();
                }
                else if (curToken.TokenType == TokenType.Delimiter_Dot)
                {
                    NextToken();
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expectedTokens">Tokens que espera-se encontrar</param>
        /// <param name="skipUntil">Caso a condição não seja satisfeita, será ignorado todos os token 
        /// até que algum dos especificados neste parâmetro seja encontrado, isso a chave para que possa-se 
        /// continuar o processamento do código para se encontrar o máximo de erros numa só verificação.</param>
        /// <returns>Satisfez a condição, encontrou o token esperado?</returns>
        private bool Expected_Token(TokenType[] expectedTokens, ErrorLocation errorLocation, SyntaxNode calledFromNode, bool panicError = false)
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

            if (!result)
            {
                Error("Expected " + ParserUtils.GetExpectedTokenListAsString(expectedTokens), curToken
                      ,errorLocation, calledFromNode, panicError);
                if (!panicError)
                {
                    NextToken();
                }
            }

            return result;
        }

        private bool Expected_Token(TokenType expectedToken, ErrorLocation errorLocation, SyntaxNode calledFromNode, bool panicError = false)
        {
            return Expected_Token(new TokenType[] { expectedToken }, errorLocation, calledFromNode, panicError);
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
                    msg = "Error: ";
                    break;
                case LogInfo.Info:
                    msg = "Info: ";
                    break;
                default:
                    break;
            }

            msg += message;
            Logger.Log(msg, this);
        }

        private void Error(string message, Token errorToken, ErrorLocation errorLocation, SyntaxNode node = null, bool panicError = false)
        {
            Log(message + " At: " + errorToken.ToString(), LogInfo.Error);

            ParserError codeError = new ParserError(this, errorToken, message);
            _compilationUnit.Errors.Add(codeError);
            ProcessError(errorToken, errorLocation, node, panicError);
        }

        private void ProcessError(Token errorToken, ErrorLocation errorLocation, SyntaxNode node = null, bool panicError = false)
        {
            switch (errorLocation)
            {
                case ErrorLocation.CISEsBody:
                    {
                      /*  while(curToken.Value != Grammar.Delimiters.OBlock.Value)
                        {
                            NextToken();
                        }*/
                        break;
                    }
                case ErrorLocation.CISEsDclr:
                    break;
                case ErrorLocation.Expected_Token:
                    break;
                case ErrorLocation.MethodBody:
                    break;
                case ErrorLocation.MethodDclr:
                    break;
                case ErrorLocation.NamespaceBody:
                    break;
                case ErrorLocation.NamespaceDclr:
                    break;
                case ErrorLocation.None:
                    break;
            }
        }

        private bool AnyToken(Token token, TokenType[] expected)
        {
            for (int i = 0; i < expected.Length; i++)
            {
                if (token.TokenType == expected[i])
                {
                    return true;
                }
            }
            return false;
        }
        /*
        private void SkipUntilEvenClosingBracket(params TokenType[] expected)
        {
            int openedByNow = 1;

            while (openedByNow != 0)
            {
                if (AnyToken(curToken, expected))
                {
                    Expected_Token(TokenType.Delimiter_CBlock);
                }
                switch (curToken.TokenType)
                {
                    case TokenType.Delimiter_OBlock:
                        openedByNow++;
                        break;
                    case TokenType.Delimiter_CBlock:
                        openedByNow--;
                        break;
                }

                reader.Read();
                curToken = reader.Peek();
                Debug(true, "tchan invocadaun");
            }
        }
        */
        private TokenType[] UnionTokenTypes(params object[] Params)
        {
            List<TokenType> allToken = new List<TokenType>();
            for (int i = 0; i < Params.Length; i++)
            {
                if (Params[i].GetType() == typeof(TokenType[]))
                {
                    allToken.AddRange((TokenType[])Params[i]);
                }
                else if (Params[i].GetType() == typeof(TokenType))
                {
                    allToken.Add((TokenType)Params[i]);
                }
                else
                {
                    throw new Exception();
                }
            }

            return allToken.ToArray();
        }
        private enum LogInfo
        {
            Created,
            Processing,
            Detected,
            Error,
            Info
        }

        public enum ErrorLocation
        {
            None,
            AccessModifier,
            AssignExpr,
            CISEsDclr,
            CISEsBody,
            DataType,
            MethodDclr,
            MethodBody,
            NamespaceDclr,
            NamespaceBody,
            ParameterDclr,
            VariableDclr,
            VariableBody,
            Expected_Token
        }

    }
}
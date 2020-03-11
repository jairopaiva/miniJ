using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
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
        public readonly TokenReader Reader;

        public Namespace currentNamespace;
        public DataType currentCISE;
        public Method currentMethod;

        public Token curToken;

        private int _closedBlock;
        private int _openedBlocks;
        private object accessModifier;

        public PreProcessor()
        {
            Reader = new TokenReader(Helpers.Global.lexerTokenCollection);
            currentNamespace = Helpers.Global.GlobalNamespace;
            _openedBlocks = 0;
            _closedBlock = 0;
            accessModifier = null;
            currentCISE = null;
            currentMethod = null;
        }

        /// <summary>
        /// Retonar o modificador de accesso que provavelmente, já foi achado, caso não tenha sido declarado
        /// de forma explicita, a função retorna o modificador de acesso padrão para o tipo de objeto.
        /// </summary>
        /// <param name="defaultAccessModifier">Modificador de acesso padrão caso não o tenha</param>
        /// <returns>Modificador de acesso</returns>
        public AccessModifierEnum GetAccessModifier(AccessModifierEnum defaultAccessModifier)
        {
            if (accessModifier != null)
            {
                if (accessModifier.GetType() == typeof(Token))
                {
                    AccessModifierEnum result = ParserUtils.GetAccessModifier((accessModifier as Token).TokenType);
                    accessModifier = null;
                    return result;
                }
                else
                    Error("GetAccessModifier");
            }

            return defaultAccessModifier;
        }

        public void Process()
        {
            curToken = Reader.Peek();
            while (curToken.TokenType != TokenType.Delimiter_EOF)
            {
                if (curToken.TokenType == TokenType.Keyword_Namespace)
                {
                    ProcessNamespace();
                }
                else if (curToken.IsToken(Token.ACCESS_MODIFIER))
                {
                    ProcessAccessModifier();
                }
                else if (ParserUtils.IsCISE(curToken))
                {
                    ProcessCISE();
                }
                else if (curToken.TokenType == TokenType.Keyword_Using)
                {
                    ProcessUsing();
                }
                else
                {
                    Error("");
                }
            }
        }

        private AssignmentExpression ParseAssignmentExpr()
        {
            return null;
        }

        private void ParseMethod(Token name, Token type)
        {
            switch (currentCISE.SpecificType)
            {
            }
        }

        private void ParseMethodParametrsDclr(Token name, Token type, Method curMethod = null)
        {
            Next(); // (
            List<ParameterDeclaration> parameters = new List<ParameterDeclaration>();
            if (curMethod == null)
            {
                curMethod = currentMethod;
            }
            {
                Expected_Token(false, TokenType.NotDef_TypeIdentifier);
            }
            Next();
            switch (curToken.TokenType)
            {
                case TokenType.Delimiter_OBlock:
                    if (curMethod.CISE.SpecificType == DataType.SpecificTypeOfData.Interface)
                    {
                        Error("Can't declare method body in an interface!");
                    }
                    break;

                default:
                    if (curMethod.CISE.SpecificType == DataType.SpecificTypeOfData.Interface)
                    {
                        Expected_Token(true, TokenType.Delimiter_CInstruction);
                    }
                    else
                    {
                        Expected_Token(true, TokenType.Delimiter_CInstruction);
                    }
                    break;
            }
            curMethod.Parameters = parameters;
        }

        private Variable ParseVariableDeclaration(bool methodReturn = false, bool parameterDclr = false)
        {
            Expected_Token(true, TokenType.NotDef_TypeIdentifier);
            Token type = curToken;
            return null;
        }

        private void Debug(bool readKey = false, string text = "", Token token = null)
        {
            if (token != null)
                Console.WriteLine(text + token.ToString());
            else
                Console.WriteLine(text + curToken.ToString());
            if (readKey)
                Console.ReadKey();
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
                Error("Expected " + ParserUtils.GetExpectedTokenListAsString(expectedTokens) + " at " + curToken.Location.ToString());

            return result;
        }

        private void Log(string info, bool readKey = false)
        {
            Console.WriteLine(info);
            if (readKey)
                Console.ReadKey();
        }

        private void Next()
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

        private void ProcessAccessModifier()
        {
            if (accessModifier == null) // Esperado, caso contrário, algo deu errado previamente
            {
                accessModifier = curToken;
                Next();
            }
            else
                Error("Mais de um modificador de acesso para o mesmo objeto.");
        }

        private void ProcessCISE()
        {
            switch (curToken.TokenType)
            {
                case TokenType.Keyword_Class:
                    ProcessClass();
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

        private void ProcessClass()
        {
            Next(); // class
            Class Classe = new Class(curToken.Value, curToken);
            Classe.AccessModifier = GetAccessModifier(AccessModifierEnum.Private);
            Next();
            if (Expected_Token(false, TokenType.Delimiter_Collon))
            {
                while (curToken.TokenType != TokenType.Delimiter_OBlock)
                    Next();
            }

            currentNamespace.CISEs.Add(Classe.Name, Classe);
            currentCISE = currentNamespace.CISEs[Classe.Name];

            ProcessClassBody();
        }

        private void ProcessClassBody()
        {
            Next(); // class
            Class Classe = new Class(curToken.Value, curToken);
            Classe.AccessModifier = GetAccessModifier(AccessModifierEnum.Private);
            Next();
            if (Expected_Token(false, TokenType.Delimiter_Collon))
            {
                while (curToken.TokenType != TokenType.Delimiter_OBlock)
                    Next();
            }
            currentNamespace.CISEs.Add(Classe.Name, Classe);
            currentCISE = currentNamespace.CISEs[Classe.Name];
            Classe.Namespace = currentNamespace;
            Log("Added class: " + Classe.Name + " with '" + Classe.AccessModifier.ToString() + "' access in namespace '" + Classe.Namespace.ToString() + "'.");
            ProcessClassBody();
        }

        private Variable ParseVariableDeclaration(Token name, Token type)
        {
            Variable variable = new Variable(name);
            // variable.Type = new DataType(type.Value, DataType.SpecificTypeOfData.)

            return variable;
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
                        Error("Identificador inválido!");
                    }
                    result.Add(curToken);
                }
                Next();
            }
            return result;
        }

        private void ProcessEnum()
        {
        }

        private void ProcessInterface()
        {
        }

        private void ProcessNamespace()
        {
            Next(); // namespace
            if (curToken.TokenType == TokenType.Delimiter_OBlock) // namespace com nome vazio
                goto namespaceGlobal;
            List<Token> namespaceName = ProcessDotExpr(TokenType.Delimiter_OBlock);
            Namespace curGlobal = null;
            if (_openedBlocks != _closedBlock)
                curGlobal = currentNamespace.Clone();
            else
                curGlobal = Helpers.Global.GlobalNamespace.Clone();
            foreach (Token nameToken in namespaceName)
            {
                if (curGlobal.Childs.Exists(n => n.Name == nameToken.Value))
                {
                    curGlobal = curGlobal.Childs.Find(n => n.Name == nameToken.Value);
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
            currentNamespace = curGlobal;
            Log("Created namespace: " + currentNamespace.ToString());
        //  Log("Created namespace: " + ParserUtils.GetDotExpr(namespaceName));
        verificaNamespaceVazio:
            Next(); // {
            if (curToken.TokenType == TokenType.Delimiter_CBlock) // Namespace vazio
            {
                Error("Cannot declare empthy namespaces!");
            }
            return;
        namespaceGlobal: // o namespace foi declarado sem nome, ou seja, tudo dentro dele fará parte de 'global'
            currentNamespace = Helpers.Global.GlobalNamespace.Clone();
            goto verificaNamespaceVazio;
        }

        private void ProcessStruct()
        {
        }

        private void ProcessUsing()
        {
            Next(); // using
            List<Token> usingPath = ProcessDotExpr(TokenType.Delimiter_CInstruction, true);
            Log("Using at: " + currentNamespace.Name + " - Complete path: " + ParserUtils.GetDotExpr(usingPath));
            Next(); // ;
        }

        private void Error(string message)
        {
            Debug(true, "Error: " + message);
            throw new Exception();
        }
    }
}
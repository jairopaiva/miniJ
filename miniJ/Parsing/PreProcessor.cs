using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical;
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
                    throw new Exception();
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
                else if (ParserUtils.IsAccessModifier(curToken))
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
                    throw new System.Exception();
                }
            }
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
        private bool Expected_Token(TokenType expectedToken, bool throwable = true)
        {
            bool result = true;
            if (curToken.TokenType != expectedToken)
            {
                result = false;
                if (throwable)
                    throw new Exception("Expected '" + LexerUtils.GetTokenValueByType(expectedToken) + "' at " + curToken.Location.ToString());
            }
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
                throw new Exception();
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
            if (Expected_Token(TokenType.Delimiter_Collon, false))
            {
                while (curToken.TokenType != TokenType.Delimiter_OBlock)
                    Next();
            }

            Expected_Token(TokenType.Delimiter_OBlock);
            Next(); // {
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
                        throw new Exception();
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
                curGlobal = currentNamespace;
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

            Log("Created namespace: " + ParserUtils.GetDotExpr(namespaceName));

        verificaNamespaceVazio:
            Next(); // {
            if (curToken.TokenType == TokenType.Delimiter_CBlock) // Namespace vazio
            {
                throw new Exception("Cannot declare empthy namespaces!");
            }
            return;

        namespaceGlobal: // o namespace foi declarado sem nome, ou seja, tudo dentro dele fará parte de 'global'
            currentNamespace = Helpers.Global.GlobalNamespace;
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
    }
}
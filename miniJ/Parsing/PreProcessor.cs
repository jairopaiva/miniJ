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

        public Namespace currentNamespace = Helpers.Global.GlobalNamespace;

        public Token curToken;

        public PreProcessor()
        {
            Reader = new TokenReader(Helpers.Global.lexerTokenCollection);
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

        private void ExpectToken(TokenType expectedToken)
        {
            if (curToken.TokenType != expectedToken)
                throw new Exception("Expected '" + LexerUtils.GetTokenValueByType(expectedToken) + "' at " + curToken.Location.ToString());
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
        }

        private void ProcessAccessModifier()
        {
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
            Next();

            List<Token> namespaceName = ProcessDotExpr(TokenType.Delimiter_OBlock);
            Namespace curGlobal = Helpers.Global.GlobalNamespace.Clone();

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

            Log("Created namespace: " + ParserUtils.GetDotExpr(namespaceName));
            ExpectToken(TokenType.Delimiter_OBlock);
            Next();
        }

        private void ProcessStruct()
        {
        }

        private void ProcessUsing()
        {
        }
    }
}
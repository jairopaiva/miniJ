using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using System;
using System.Collections.Generic;

namespace miniJ.Parsing
{
    class PreProcessor : ICompilerNode
    {
        public PreProcessor()
        {
            Reader = new TokenReader(Helpers.Global.lexerTokenCollection);
        }

        public Namespace currentNamespace = Helpers.Global.GlobalNamespace;
        public readonly TokenReader Reader;

        public Token curToken;

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
                else
                {
                    throw new System.Exception();
                }
            }
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

        private void ProcessAccessModifier()
        {
        }

        private void ProcessNamespace()
        {
            Debug("Creating namespace: ");
            Next();

            List<Token> namespaceName = ProcessDotExpr(TokenType.Delimiter_OBlock);

            Log("Finished namespace declaration",true);

        }

        private void ProcessClass()
        {
        }

        private void ProcessInterface()
        {
        }

        private void ProcessStruct()
        {
        }

        private void ProcessEnum()
        {
        }

        private List<Token> ProcessDotExpr(TokenType delimiter)
        {
            List<Token> result = new List<Token>();
            while(curToken.TokenType != delimiter)
            {
                if (curToken.TokenType != TokenType.Delimiter_Dot)
                {
                    result.Add(curToken);
                }
                Next();
            }
            return result;
        }

        private void Next()
        {
            Reader.Read();
            curToken = Reader.Peek();
        }

        private void Debug(string text = "", Token token = null)
        {
            if (token != null)
                Console.WriteLine(text + token.ToString());
            else
                Console.WriteLine(text + curToken.ToString());
        }

        private void Log(string info, bool readKey = false)
        {
            Console.WriteLine(info);
            if (readKey)
                Console.ReadKey();
        }
    }
}
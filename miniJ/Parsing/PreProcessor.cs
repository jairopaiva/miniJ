using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;

namespace miniJ.Parsing
{
    class PreProcessor : ICompilerNode
    {
        public PreProcessor()
        {
            Global = Helpers.Global.GlobalNamespace;
            Reader = new TokenReader(Helpers.Global.lexerTokenCollection);
        }

        public readonly Namespace Global;
        public readonly TokenReader Reader;

        public Token PeekToken;

        public void Process()
        {
            PeekToken = Reader.Peek();
            while (PeekToken.TokenType != TokenType.Delimiter_EOF)
            {
                if (PeekToken.TokenType == TokenType.Keyword_Namespace)
                {
                    ProcessNamespace();
                }
                else if (ParserUtils.IsAccessModifier(PeekToken))
                {
                    ProcessAccessModifier();
                }
                else if (ParserUtils.IsCISE(PeekToken))
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
            switch (PeekToken.TokenType)
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
    }
}
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing
{
    class ParserUtils
    {
        public static bool IsCISE(Token token)
        {
            return token.TokenType == TokenType.Keyword_Class ||
                token.TokenType == TokenType.Keyword_Interface ||
                token.TokenType == TokenType.Keyword_Struct ||
                token.TokenType == TokenType.Keyword_Enum;
        }

        public static bool IsAccessModifier(Token token)
        {
            return token.TokenType == TokenType.AccessModifier_Private ||
                token.TokenType == TokenType.AccessModifier_Protected ||
                token.TokenType == TokenType.AccessModifier_Public;
        }
    }
}
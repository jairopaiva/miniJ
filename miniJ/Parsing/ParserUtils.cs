using miniJ.Grammar;
using miniJ.Lexical;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Parsing
{
    class ParserUtils
    {
        public static AccessModifierEnum GetAccessModifier(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.AccessModifier_Private:
                    return AccessModifierEnum.Private;

                case TokenType.AccessModifier_Protected:
                    return AccessModifierEnum.Protected;

                case TokenType.AccessModifier_Public:
                    return AccessModifierEnum.Public;

                default:
                    throw new Exception();
            }
        }

        public static string GetExpectedTokenListAsString(TokenType[] list)
        {
            StringBuilder builder = new StringBuilder('[');
            for (int i = 0; i < list.Length; i++)
            {
                string tokenValue = LexerUtils.GetTokenValueByType(list[i]);
                if (i == list.Length - 1)
                {
                    builder.Append(tokenValue + ']');
                }
                else
                {
                    builder.Append(tokenValue + ", ");
                }
            }

            return builder.ToString();
        }

        public static string GetDotExpr(List<Token> tokens)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (i == tokens.Count - 1)
                {
                    builder.Append(tokens[i].Value);
                }
                else
                {
                    builder.Append(tokens[i].Value);
                    builder.Append(Delimiters.Dot.Value);
                }
            }
            return builder.ToString();
        }

        public static bool IsCISE(Token token)
        {
            return token.TokenType == TokenType.Keyword_Class ||
                token.TokenType == TokenType.Keyword_Interface ||
                token.TokenType == TokenType.Keyword_Struct ||
                token.TokenType == TokenType.Keyword_Enum;
        }

        public static bool ValidIdentifier(string ID)
        {
            if (ID[0] == LexerUtils.UNDERLINE || ID[ID.Length - 1] == LexerUtils.UNDERLINE)
                return false;

            for (int i = 0; i < ID.Length; i++)
                if (!char.IsDigit(ID[i]) && !char.IsLetter(ID[i]) && ID[i] != LexerUtils.UNDERLINE)
                    return false;
            return true;
        }
    }
}
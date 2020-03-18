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
    {/*
        public static AccessModifierNode GetAccessModifier(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.AccessModifier_Private:
                    return AccessModifierNode.Private;

                case TokenType.AccessModifier_Protected:
                    return AccessModifierNode.Protected;

                case TokenType.AccessModifier_Public:
                    return AccessModifierNode.Public;

                default:
                    throw new Exception();
            }
        }
        */
        public static string GetExpectedTokenListAsString(TokenType[] list)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');

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
            switch (token.TokenType)
            {
                case TokenType.Keyword_Class:
                case TokenType.Keyword_Interface:
                case TokenType.Keyword_Struct:
                case TokenType.Keyword_Enum:
                    return true;
                default:
                    return false;
            }
        }

        public static CISE.SpecificTypeOfCISE GetCISEType(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.Keyword_Class:
                    return CISE.SpecificTypeOfCISE.Class;
                case TokenType.Keyword_Interface:
                    return CISE.SpecificTypeOfCISE.Interface;
                case TokenType.Keyword_Struct:
                    return CISE.SpecificTypeOfCISE.Struct;
                case TokenType.Keyword_Enum:
                    return CISE.SpecificTypeOfCISE.Enum;
                default:
                    throw new Exception();
            }
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
using miniJ.Grammar;
using miniJ.Helpers;
using miniJ.Lexical.Elements.Token;
using System.Linq;

namespace miniJ.Lexical
{
    public static class LexerUtils
    {
        public const char NEWLINE = '\n';
        public const char UNDERLINE = '_';
        public static readonly char[] HEX_SIGNAL = { '0', 'x' };

        public static string GetTokenValueByType(TokenType tokenType)
        {
            Token token = Global.tokenDatabase.FirstOrDefault(token => token.Value.TokenType == tokenType).Value;
            if (token != null)
            {
                if (token.Value != null)
                {
                    return token.Value;
                }
            }

            return tokenType.ToString();
        }

        public static bool HexChar(char c)
        {
            int charCode = (int)c;

            if (!char.IsDigit(c) && charCode < 97 || charCode > 102)
                return false;
            return true;
        }

        public static bool IsNewLine(char c)
        {
            return c == NEWLINE;
        }

        public static bool IsOperatorOrComparator(string curChar)
        {
            if (Global.tokenDatabase.ContainsKey(curChar))
                return Global.tokenDatabase[curChar].TokenType.ToString().StartsWith("Operator") ||
                        Global.tokenDatabase[curChar].TokenType.ToString().StartsWith("Comparator");
            return false;
        }

        public static bool OnlyNumber(string value)
        {
            int i = 0;
            if (value[i] == Operators.Add.Value[0] || value[i] == Operators.Sub.Value[0])
                i++;

            for (; i < value.Length; i++)
                if (!char.IsDigit(value[i]))
                    return false;
            return true;
        }
    }
}
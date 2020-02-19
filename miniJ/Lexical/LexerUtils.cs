using miniJ.Grammar;
using miniJ.Helpers;

namespace miniJ.Lexical
{
    public static class LexerUtils
    {
        public static readonly char[] HEX_SIGNAL = { '0', 'x' };
        public const char NEWLINE = '\n';
        public const char UNDERLINE = '_';

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

        public static bool IsOperatorOrComparator(string curChar)
        {
            if (Global.tokenDatabase.ContainsKey(curChar))
                return Global.tokenDatabase[curChar].TokenType.ToString().StartsWith("Operator") ||
                        Global.tokenDatabase[curChar].TokenType.ToString().StartsWith("Comparator");
            return false;
        }

        public static bool IsNewLine(char c)
        {
            return c == NEWLINE;
        }

        public static bool HexChar(char c)
        {
            int charCode = (int)c;
            char dfebug = (char)charCode;

            if (!char.IsDigit(c) && charCode < 97 || charCode > 102)
                return false;
            return true;
        }

        public static bool ValidIdentifier(string ID)
        {
            if (ID[0] == UNDERLINE || ID[ID.Length - 1] == UNDERLINE)
                return false;

            for (int i = 0; i < ID.Length; i++)
                if (!char.IsDigit(ID[i]) && !char.IsLetter(ID[i]) && ID[i] != UNDERLINE)
                    return false;
            return true;
        }
    }
}
using System.Collections.Generic;

namespace miniJ.Lexical
{
    public static class LexerUtils
    {
        public static readonly char[] HEX_SIGNAL = { '0', 'x' };
        public const char DOT = '.';
        public const char DIRECTIVE_SIGNAL = '#';
        public const char NEWLINE = '\n';
        public const char UNDERLINE = '_';
        public const char MINUS = '-';
        public const char PLUS = '+';

        public static bool OnlyNumber(string value)
        {
            for (int i = 0; i < value.Length; i++)
                if (!char.IsDigit(value[i]) && value[i] != MINUS && value[i] != PLUS)
                    return false;
            return true;
        }

        public static bool IsNewLine(char c)
        {
            return c == NEWLINE;
        }

        public static bool HexChar(char c)
        {
            List<char> hex = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f' };

            if (!char.IsDigit(c) && !hex.Contains(c))
                return false;
            return true;
        }

        public static bool ValidIdentifier(string ID)
        {
            for (int i = 0; i < ID.Length; i++)
                if (!char.IsDigit(ID[i]) && !char.IsLetter(ID[i]) && ID[i] != UNDERLINE)
                    return false;
            return true;
        }
    }
}
using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;

namespace miniJ.Parsing.Elements
{
    class TokenReader
    {
        private readonly List<Token> Tokens;

        private int tempPos = 0;

        public TokenReader(List<Token> tokens)
        {
            Tokens = tokens;
        }

        public int Position;

        public Token this[int Index]
        {
            get
            {
                return Tokens[Index];
            }
        }

        public Token ReadTemp()
        {
            return Tokens[tempPos++];
        }

        public Token Peek()
        {
            if (Position == Tokens.Count - 1)
                return Grammar.Delimiters.EOF;
            return Tokens[Position];
        }

        public Token PeekTemp()
        {
            return Tokens[tempPos];
        }

        public Token Previous()
        {
            return Tokens[tempPos--];
        }

        public Token Read()
        {
            if (Position == Tokens.Count - 1)
                return Grammar.Delimiters.EOF;
            return Tokens[Position++];
        }

        public void Reset()
        {
            tempPos = Position;
        }
    }
}
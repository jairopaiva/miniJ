using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elememts;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Parsing.Elements
{
    class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(LexerToken lexerToken)
        {
            this.LexerToken = lexerToken;
        }

    }
}

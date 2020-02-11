using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elememts;

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
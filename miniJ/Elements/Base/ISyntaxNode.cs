using miniJ.Lexical.Elements.Token;

namespace miniJ.Elements.Base
{
    class ISyntaxNode
    {
        public ISyntaxNode(Token origin)// = null)
        {
            Origin = origin;
        }

        public Token Origin { get; set; }
    }
}
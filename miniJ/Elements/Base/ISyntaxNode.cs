using miniJ.Lexical.Elements.Token;

namespace miniJ.Elements.Base
{
    class ISyntaxNode
    {
        public ISyntaxNode(Token origin)
        {
            Origin = origin;
        }

        public Token Origin { get; set; }
    }
}
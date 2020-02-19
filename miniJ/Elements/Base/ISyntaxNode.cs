using miniJ.Lexical.Elements.Token;

namespace miniJ.Elements.Base
{
    class ISyntaxNode
    {
        public Token Origin { get; set; }

        public ISyntaxNode(Token origin)
        {
            Origin = origin;
        }
    }
}
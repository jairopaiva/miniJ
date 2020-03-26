using miniJ.Lexical.Elements.Token;

namespace miniJ.Elements.Base
{
     class ISyntaxNode
    {
        public Token Origin;
        public ISyntaxNode(Token origin)// = null)
        {
            Origin = origin;
        }
    }
}
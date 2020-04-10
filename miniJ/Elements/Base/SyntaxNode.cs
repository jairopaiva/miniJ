using miniJ.Lexical.Elements.Token;

namespace miniJ.Elements.Base
{
    public class SyntaxNode
    {
        public Token Origin;
        public SyntaxNode(Token origin)
        {
            Origin = origin;
        }
    }
}
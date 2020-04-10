using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    public class AssignmentExpression : Expression
    {
        public SyntaxNode ParentNode;
        public AssignmentExpression(Token origin) : base(origin)
        {
        }
    }
}
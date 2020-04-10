using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;

namespace miniJ.Parsing.Elements
{
    public class Expression : SyntaxNode
    {
        public Expression(Token origin) : base(origin)
        {
            Nodes = new List<SyntaxNode>();
        }

        public List<SyntaxNode> Nodes;
    }
}
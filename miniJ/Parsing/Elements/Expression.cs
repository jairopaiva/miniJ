using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;

namespace miniJ.Parsing.Elements
{
    class Expression : ISyntaxNode
    {
        public Expression(Token origin) : base(origin)
        {
            Nodes = new List<ISyntaxNode>();
        }

        public List<ISyntaxNode> Nodes { get; set; }
    }
}
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class Method : ISyntaxNode
    {
        public Method(Token origin) : base(origin)
        {
        }

        public Variable ReturnType { get; set; }
    }
}
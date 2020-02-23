using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class Variable : ISyntaxNode
    {
        public Variable(Token origin) : base(origin)
        {
        }

        public TypeConfiguration Configuration { get; set; }
        public DataType Type { get; set; }
        public Expression Value { get; set; }
    }
}
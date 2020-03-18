using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    /// <summary>
    /// Inside method
    /// </summary>
    class Variable : ISyntaxNode
    {
        public Token Name { get; set; }
        public bool Constant { get; set; }
        public DataType Type { get; set; }
        public Expression Value { get; set; }
        public Variable(Token origin) : base(origin)
        {
        }

        public override string ToString()
        {
            return this.GetType().Name + " named as " + Name + ", of " + Type.Name + " type. Has value = "
              + (Value != null);
        }
    }
}
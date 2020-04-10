using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements.DataTypes;

namespace miniJ.Parsing.Elements.Symbols
{
    /// <summary>
    /// Inside method
    /// </summary>
    public class Variable : SyntaxNode
    {
        public string Name;
        public DataType Type;
        public Expression Value;
        public Variable(Token origin) : base(origin)
        {
        }

        public override string ToString()
        {
            return this.GetType().Name + " named as " + Name + ", of " + Type.Name + " type. Assigned = "
              + (Value != null);
        }
    }
}
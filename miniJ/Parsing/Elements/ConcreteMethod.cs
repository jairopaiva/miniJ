using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements.DataTypes;

namespace miniJ.Parsing.Elements
{
    class ConcreteMethod : AbstractMethod
    {
        public Body Body;
        public ConcreteMethod(Token origin, DataType returnType, AccessModifierNode accessModifier) : base(origin, returnType, accessModifier)
        {
        }
    }
}
using miniJ.Parsing.Elements.DataTypes;

namespace miniJ.Parsing.Elements
{
    class ConcreteMethod : AbstractMethod
    {
        public Body Body;
        public ConcreteMethod(string name, DataType returnType, AccessModifierNode accessModifier) : base(name, returnType, accessModifier)
        {
        }
    }
}
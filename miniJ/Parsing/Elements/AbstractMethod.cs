using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements.DataTypes;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Parsing.Elements
{
    class AbstractMethod : ISyntaxNode
    {
        public CISE CISE;
        public List<ParameterDeclaration> Parameters;
        public DataType ReturnType;
        public string Name;
        public AccessModifierNode AccessModifier;
        public AbstractMethod(Token origin, DataType returnType, AccessModifierNode accessModifier) : base(origin)
        {
            Name = origin.Value;
            ReturnType = returnType;
            AccessModifier = accessModifier;
            Parameters = new List<ParameterDeclaration>();
        }

        public override string ToString()
        {
            return this.GetType().Name + " returning " + ReturnType.Name + ", named as " +
                Name + ", inside " + CISE.Name;
        }
    }
}
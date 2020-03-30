using miniJ.Elements.Base;
using miniJ.Parsing.Elements.DataTypes;
using System.Collections.Generic;

namespace miniJ.Parsing.Elements
{
    class AbstractMethod : ISyntaxNode
    {
        public List<ParameterDeclaration> Parameters;
        public AccessModifierNode AccessModifier;
        public DataType ReturnType;
        public bool Virtual;
        public string Name;
        public CISE CISE;
        
        public AbstractMethod(string name, DataType returnType, AccessModifierNode accessModifier) : base(returnType.Origin)
        {
            Name = name;
            ReturnType = returnType;
            AccessModifier = accessModifier;
            Parameters = new List<ParameterDeclaration>();
            Virtual = false;
        }

        public override string ToString()
        {
            return this.GetType().Name + " returning " + ReturnType.Name + ", named as " +
                Name + ", inside " + CISE.Name;
        }
    }
}
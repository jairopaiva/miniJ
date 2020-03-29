using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
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
        
        public AbstractMethod(Token origin, DataType returnType, AccessModifierNode accessModifier) : base(origin)
        {
            Name = origin.Value;
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
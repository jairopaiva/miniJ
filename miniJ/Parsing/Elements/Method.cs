using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Parsing.Elements
{
    class Method : ISyntaxNode
    {
        public CISE CISE { get; set; }
        public List<ParameterDeclaration> Parameters { get; set; }
        public DataType ReturnType { get; set; }
        public Token Name { get; set; }
        public Body Body { get; set; }
        public AccessModifierNode AccessModifier { get; set; }
        public Method(Token origin, DataType returnType, AccessModifierNode accessModifier) : base(origin)
        {
            Name = origin;
            ReturnType = returnType;
            AccessModifier = accessModifier;
            Parameters = new List<ParameterDeclaration>();
        }

        public override string ToString()
        {
            return this.GetType().Name + " returning " + ReturnType.Name + ", named as " +
                Name.Value + ", inside " + CISE.Name.Value;
        }
    }
}
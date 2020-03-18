
using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System;

namespace miniJ.Parsing.Elements
{
    class CISE : ISyntaxNode
    {

        public Token Name { get; set; }
        public SpecificTypeOfCISE TypeOfCISE;
        public Namespace Namespace { get; set; }
        public AccessModifierNode AccessModifier { get; set; }
        public CISE(Token name, SpecificTypeOfCISE type, Token origin) : base(origin)
        {
            if (ParserUtils.ValidIdentifier(name.Value))
                Name = name;
            else
                throw new Exception();
            TypeOfCISE = type;
        }

        public enum SpecificTypeOfCISE
        {
            Class,
            Interface,
            Struct,
            Enum
        }

        public override string ToString()
        {
            return TypeOfCISE + ", " +
                Name.Value + ", inside " + Namespace.Name.Value;
        }

    }
}
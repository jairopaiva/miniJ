using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing;
using miniJ.Parsing.Elements;
using System.Collections.Generic;

namespace miniJ.Elements
{
    class DataType : ISyntaxNode
    {
        public DataType(string name, SpecificTypeOfData type, Token origin) : base(origin)
        {
            if (ParserUtils.ValidIdentifier(name))
                Name = name;
            SpecificType = type;
            Variables = new List<Variable>();
        }

        public enum SpecificTypeOfData
        {
            Class,
            Interface,
            Struct,
            Enum,
            PrimitiveType
        }

        public string Name { get; set; }
        public SpecificTypeOfData SpecificType { get; set; }
        public List<Variable> Variables { get; set; }
    }
}
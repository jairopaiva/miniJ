using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing;

namespace miniJ.Elements
{
    class DataType : ISyntaxNode
    {
        public DataType(string name, SpecificTypeOfData type, Token origin) : base(origin)
        {
            if (ParserUtils.ValidIdentifier(name))
                Name = name;
            SpecificType = type;
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
    }
}
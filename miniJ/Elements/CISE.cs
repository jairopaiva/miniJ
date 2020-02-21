using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Elements
{
    class CISE : ISyntaxNode
    {
        public CISE(string name, CISEType type, Token origin) : base(origin)
        {
            Name = name;
            Type = type;
        }

        public enum CISEType
        {
            Class,
            Interface,
            Struct,
            Enum
        }

        public string Name { get; set; }
        public Namespace RefNamespace { get; set; }
        public CISEType Type { get; set; }
    }
}
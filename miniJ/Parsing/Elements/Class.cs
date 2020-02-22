using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;

namespace miniJ.Elements
{
    class Class : CISE
    {
        public Class(string name, Token origin) : base(name, CISEType.Class, origin)
        {
        }

        public AccessModifierEnum AccessModifier { get; set; }
    }
}
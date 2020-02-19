using miniJ.Lexical.Elements.Token;

namespace miniJ.Elements
{
    class Class : CISE
    {
        public Class(string name, Token origin) : base(name, CISEType.Class, origin)
        {
        }
    }
}
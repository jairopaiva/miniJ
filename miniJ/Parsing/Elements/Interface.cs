using miniJ.Elements;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class Interface : CISE
    {
        public Interface(Token name, Token origin) : base(name, SpecificTypeOfCISE.Interface, origin)
        {
        }
    }
}
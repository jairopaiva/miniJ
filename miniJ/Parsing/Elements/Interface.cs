using miniJ.Elements;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class Interface : DataType
    {
        public Interface(string name, Token origin) : base(name, SpecificTypeOfData.Interface, origin)
        {
        }
    }
}
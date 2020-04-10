using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using System.Collections.Generic;

namespace miniJ.Elements
{
    public class Class : CISE
    {
        public Class(Token name, Token origin) : base(name, SpecificTypeOfCISE.Class, origin)
        {
            Implements = new List<Interface>();
        }

        public Class Extends;
        public List<Interface> Implements;

    }
}
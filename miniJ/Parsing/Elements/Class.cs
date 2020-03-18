using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using System.Collections.Generic;

namespace miniJ.Elements
{
    class Class : CISE
    {
        public Class(Token name, Token origin) : base(name, SpecificTypeOfCISE.Class, origin)
        {
            Implements = new List<Interface>();
        }

        public Body Body { get; set; }
        public Class Extends { get; set; }
        public List<Interface> Implements { get; set; }

    }
}
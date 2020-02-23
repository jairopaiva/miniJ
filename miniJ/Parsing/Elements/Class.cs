using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using System.Collections.Generic;

namespace miniJ.Elements
{
    class Class : DataType
    {
        public Class(string name, Token origin) : base(name, SpecificTypeOfData.Class, origin)
        {
            Implements = new List<Interface>();
        }

        public AccessModifierEnum AccessModifier { get; set; }
        public Body Body { get; set; }
        public Class Extends { get; set; }
        public List<Interface> Implements { get; set; }
        public Namespace Namespace { get; set; }
    }
}
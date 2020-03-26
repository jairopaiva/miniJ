using miniJ.Lexical.Elements.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Parsing.Elements
{
    /// <summary>
    /// Inside CISE, out method
    /// </summary>
    class Field : Variable
    {
        public AccessModifierNode AccessModifier;
        public Field(Token origin) : base(origin)
        {
        }
    }
}

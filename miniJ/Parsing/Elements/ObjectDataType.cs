using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing;
using miniJ.Parsing.Elements;
using System.Collections.Generic;

namespace miniJ.Elements
{
    class ObjectDataType : DataType
    {
        public CISE CISE { get; set; }
        public ObjectDataType(Token origin) : base(origin)
        {
        }
    }
}
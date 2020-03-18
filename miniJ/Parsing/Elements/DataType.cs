using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Parsing.Elements
{
    class DataType : ISyntaxNode
    {
        public string Name { get; set; }
        public DataTypeConfiguration Settings { get; set; }
        public DataType(Token origin) : base(origin)
        {
            Name = origin.Value;
        }

        public struct DataTypeConfiguration
        {
            public bool Array { get; set; }
            public bool Static { get; set; }
        }

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            if (Settings.Array)
                info.Append("Array of ");
            info.Append(Name);
            return info.ToString();
        }
    }
}

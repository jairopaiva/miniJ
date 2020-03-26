using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System.Text;

namespace miniJ.Parsing.Elements.DataTypes
{
    class DataType : ISyntaxNode
    {
        public string Name;
        public DataTypeConfiguration Settings;
        public DataType(Token origin) : base(origin)
        {
            Name = origin.Value;
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

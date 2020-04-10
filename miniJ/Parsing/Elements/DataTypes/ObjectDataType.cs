using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements.DataTypes
{
    class ObjectDataType : DataType
    {
        public CISE CISE;
        public ObjectDataType(Token origin) : base(origin)
        {
        }
    }
}
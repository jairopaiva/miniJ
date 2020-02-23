using miniJ.Elements;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class PrimitiveType : DataType
    {
        public static PrimitiveType Bool = new PrimitiveType(Grammar.Types.Bool);

        public static PrimitiveType Byte = new PrimitiveType(Grammar.Types.Byte);

        public static PrimitiveType Char = new PrimitiveType(Grammar.Types.Char);

        public static PrimitiveType Double = new PrimitiveType(Grammar.Types.Double);

        public static PrimitiveType Float = new PrimitiveType(Grammar.Types.Float);

        public static PrimitiveType Int = new PrimitiveType(Grammar.Types.Int);

        public static PrimitiveType Long = new PrimitiveType(Grammar.Types.Long);

        public static PrimitiveType String = new PrimitiveType(Grammar.Types.String);

        public static PrimitiveType Void = new PrimitiveType(Grammar.Types.Void);

        private PrimitiveType(Token origin) : base(origin.Value, SpecificTypeOfData.PrimitiveType, origin)
        {
        }

        public PrimitiveType Clone(NodeLocation location)
        {
            PrimitiveType dataType = new PrimitiveType(Origin);
            dataType.Origin.Location = location;
            return dataType;
        }
    }
}
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;

namespace miniJ.Parsing.Elements.DataTypes
{
    class PrimitiveDataType : DataType
    {
        
        #region"Built In Primitive Types"
        /*
        public static PrimitiveDataType PRIMITIVE_DATATYPE__BOOL =
           new PrimitiveDataType(BuiltInType_.Bool);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__BYTE =
           new PrimitiveDataType(BuiltInType_.Byte);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__CHAR =
           new PrimitiveDataType(BuiltInType_.Char);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__DOUBLE =
           new PrimitiveDataType(BuiltInType_.Double);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__FLOAT =
           new PrimitiveDataType(BuiltInType_.Float);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__INT =
           new PrimitiveDataType(BuiltInType_.Int);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__LONG =
           new PrimitiveDataType(BuiltInType_.Long);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__STRING =
           new PrimitiveDataType(BuiltInType_.String);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__VOID =
            new PrimitiveDataType(BuiltInType_.Void);
            */
        #endregion
        private PrimitiveDataType(Token origin) : base(origin)
        {
        }

        public static PrimitiveDataType GetPrimitiveType(Token origin)
        {
            return new PrimitiveDataType(origin);
        }

        /*
        public static PrimitiveDataType GetPrimitiveType(Token origin)
        {
            switch (origin.TokenType)
            {
                case TokenType.BuiltInType_Bool:
                    return PRIMITIVE_DATATYPE__BOOL;
                case TokenType.BuiltInType_Byte:
                    return PRIMITIVE_DATATYPE__BYTE;
                case TokenType.BuiltInType_Char:
                    return PRIMITIVE_DATATYPE__CHAR;
                case TokenType.BuiltInType_Double:
                    return PRIMITIVE_DATATYPE__DOUBLE;
                case TokenType.BuiltInType_Float:
                    return PRIMITIVE_DATATYPE__FLOAT;
                case TokenType.BuiltInType_Int:
                    return PRIMITIVE_DATATYPE__INT;
                case TokenType.BuiltInType_Long:
                    return PRIMITIVE_DATATYPE__LONG;
                case TokenType.BuiltInType_tring:
                    return PRIMITIVE_DATATYPE__STRING;
                case TokenType.BuiltInType_Void:
                    return PRIMITIVE_DATATYPE__VOID;
                default:
                    throw new System.Exception();
            }
            */
        }
    }
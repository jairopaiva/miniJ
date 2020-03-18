using miniJ.Elements.Base;
using miniJ.Grammar;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing;
using miniJ.Parsing.Elements;
using System.Collections.Generic;

namespace miniJ.Elements
{
    class PrimitiveDataType : DataType
    {
        
        #region"Built In Primitive Types"
        /*
        public static PrimitiveDataType PRIMITIVE_DATATYPE__BOOL =
           new PrimitiveDataType(BuiltInTypes.Bool);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__BYTE =
           new PrimitiveDataType(BuiltInTypes.Byte);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__CHAR =
           new PrimitiveDataType(BuiltInTypes.Char);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__DOUBLE =
           new PrimitiveDataType(BuiltInTypes.Double);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__FLOAT =
           new PrimitiveDataType(BuiltInTypes.Float);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__INT =
           new PrimitiveDataType(BuiltInTypes.Int);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__LONG =
           new PrimitiveDataType(BuiltInTypes.Long);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__STRING =
           new PrimitiveDataType(BuiltInTypes.String);
        public static PrimitiveDataType PRIMITIVE_DATATYPE__VOID =
            new PrimitiveDataType(BuiltInTypes.Void);
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
                case TokenType.PrimitiveType_Bool:
                    return PRIMITIVE_DATATYPE__BOOL;
                case TokenType.PrimitiveType_Byte:
                    return PRIMITIVE_DATATYPE__BYTE;
                case TokenType.PrimitiveType_Char:
                    return PRIMITIVE_DATATYPE__CHAR;
                case TokenType.PrimitiveType_Double:
                    return PRIMITIVE_DATATYPE__DOUBLE;
                case TokenType.PrimitiveType_Float:
                    return PRIMITIVE_DATATYPE__FLOAT;
                case TokenType.PrimitiveType_Int:
                    return PRIMITIVE_DATATYPE__INT;
                case TokenType.PrimitiveType_Long:
                    return PRIMITIVE_DATATYPE__LONG;
                case TokenType.PrimitiveType_String:
                    return PRIMITIVE_DATATYPE__STRING;
                case TokenType.PrimitiveType_Void:
                    return PRIMITIVE_DATATYPE__VOID;
                default:
                    throw new System.Exception();
            }
            */
        }
    }
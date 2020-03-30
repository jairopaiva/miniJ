using miniJ.Grammar;
using miniJ.Lexical;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Parsing
{
    class ParserUtils
    {

        public const string CONSTRUCTOR_NAME = ".ctor";

        public static readonly TokenType[] AllCISETokens = { TokenType.Keyword_Class, TokenType.Keyword_Interface, TokenType.Keyword_Struct, TokenType.Keyword_Enum };
        public static readonly TokenType[] AllAccessTokens = { TokenType.AccessModifier_Private, TokenType.AccessModifier_Protected, TokenType.AccessModifier_Public };
        public static readonly TokenType[] AllBuiltInTokens = { TokenType.BuiltInType_Bool , TokenType.BuiltInType_Byte, TokenType.BuiltInType_Char, TokenType.BuiltInType_Double,
                                                                TokenType.BuiltInType_Float,TokenType.BuiltInType_Int, TokenType.BuiltInType_Long, TokenType.BuiltInType_String,
                                                                TokenType.BuiltInType_T,TokenType.BuiltInType_UInt,TokenType.BuiltInType_ULong,TokenType.BuiltInType_Void};

        public static string GetExpectedTokenListAsString(TokenType[] list)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');

            for (int i = 0; i < list.Length; i++)
            {
                string tokenValue = LexerUtils.GetTokenValueByType(list[i]);
                if (i == list.Length - 1)
                {
                    builder.Append(tokenValue + ']');
                }
                else
                {
                    builder.Append(tokenValue + ", ");
                }
            }

            return builder.ToString();
        }

        public static string GetDotExpr(List<Token> tokens)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (i == tokens.Count - 1)
                {
                    builder.Append(tokens[i].Value);
                }
                else
                {
                    builder.Append(tokens[i].Value);
                    builder.Append(Delimiters.Dot.Value);
                }
            }
            return builder.ToString();
        }

        public static bool IsType(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.NotDef_TypeIdentifier:
                case TokenType.BuiltInType_Bool:
                case TokenType.BuiltInType_Byte:
                case TokenType.BuiltInType_Char:
                case TokenType.BuiltInType_Double:
                case TokenType.BuiltInType_Float:
                case TokenType.BuiltInType_Int:
                case TokenType.BuiltInType_Long:
                case TokenType.BuiltInType_String:
                case TokenType.BuiltInType_T:
                case TokenType.BuiltInType_Void:

                case TokenType.Keyword_Auto:
                    return true;
                default:
                    if (Helpers.Global.lexerResult.cisesDetectedInLexer.Exists(cise => cise.Name == token.Value))
                        return true;
                    return false;
            }
        }   

        public static bool IsTypeOrModifierRelatedToType(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.Keyword_Constant:
                case TokenType.Keyword_Override:
                case TokenType.Keyword_Readonly:
                case TokenType.Keyword_Volatile:
                case TokenType.Keyword_Static:
                    return true;
                default:
                    return IsType(token);
            }
        }

        public static bool IsCISE(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.Keyword_Class:
                case TokenType.Keyword_Interface:
                case TokenType.Keyword_Struct:
                case TokenType.Keyword_Enum:
                    return true;
                default:
                    return false;
            }
        }

        public static CISE.SpecificTypeOfCISE GetCISEType(Token token)
        {
            switch (token.TokenType)
            {
                case TokenType.Keyword_Class:
                    return CISE.SpecificTypeOfCISE.Class;
                case TokenType.Keyword_Interface:
                    return CISE.SpecificTypeOfCISE.Interface;
                case TokenType.Keyword_Struct:
                    return CISE.SpecificTypeOfCISE.Struct;
                case TokenType.Keyword_Enum:
                    return CISE.SpecificTypeOfCISE.Enum;
                default:
                    throw new Exception();
            }
        }

        public static string GetSignatureModifiers(bool constant, bool readOnly, bool Volatile, bool Static, bool Virtual)
        {
            StringBuilder builder = new StringBuilder();
            if (constant)
            {
                builder.Append("[" +Keywords.Constant.Value + "] ");
            }
            if (readOnly)
            {
                builder.Append("[" + Keywords.Readonly.Value + "] ");
            }
            if (Volatile)
            {
                builder.Append("[" + Keywords.Volatile.Value + "] ");
            }
            if (Static)
            {
                builder.Append("[" + Keywords.Static.Value + "] ");
            }
            if (Virtual)
            {
                builder.Append("[" + Keywords.Virtual.Value + "] ");
            }

            return builder.ToString();
        }

        public static bool ValidIdentifier(string ID)
        {
            for (int i = 0; i < ID.Length; i++)
                if (!char.IsDigit(ID[i]) && !char.IsLetter(ID[i]) && ID[i] != LexerUtils.UNDERLINE)
                    return false;
            return true;
        }
    }
}
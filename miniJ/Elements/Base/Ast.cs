using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;

namespace miniJ.Elements.Base
{
    public class Grammar 
    {
        public static class AccessModifier
        {
            public static readonly Token Private = new Token("private", TokenType.AccessModifier_Private);
            public static readonly Token Protected = new Token("protected", TokenType.AccessModifier_Protected);
            public static readonly Token Public = new Token("public", TokenType.AccessModifier_Public);
        }

        public static class Comparators
        {
            public static readonly Token And = new Token("&&", TokenType.Comparator_And);
            public static readonly Token Different = new Token("!=", TokenType.Comparator_Different);
            public static readonly Token Equal = new Token("==", TokenType.Comparator_Equal);
            public static readonly Token GreaterThan = new Token(">", TokenType.Comparator_GreaterThan);
            public static readonly Token GreaterThanOrEqual = new Token(">=", TokenType.Comparator_GreaterThanOrEqual);
            public static readonly Token Inequality = new Token("!", TokenType.Comparator_Inequality);
            public static readonly Token LessThan = new Token("<", TokenType.Comparator_LessThan);
            public static readonly Token LessThanOrEqual = new Token("<=", TokenType.Comparator_LessThanOrEqual);
            public static readonly Token Or = new Token("||", TokenType.Comparator_Or);
        }

        public static class Delimiters
        {
            public static readonly Token Backslash = new Token(@"\", TokenType.Delimiter_Backslash);
            public static readonly Token CBlock = new Token("}", TokenType.Delimiter_CBlock);
            public static readonly Token CharAssigment = new Token("'", TokenType.Delimiter_CharAssigment);
            public static readonly Token CIndex = new Token("]", TokenType.Delimiter_CIndex);
            public static readonly Token CInstruction = new Token(";", TokenType.Delimiter_CInstruction);
            public static readonly Token Collon = new Token(":", TokenType.Delimiter_Collon);
            public static readonly Token Comma = new Token(",", TokenType.Delimiter_Comma);
            public static readonly Token CParenthesis = new Token(")", TokenType.Delimiter_CParenthesis);
            public static readonly Token Dot = new Token(".", TokenType.Delimiter_Dot);
            public static readonly Token EOF = new Token("End of File", TokenType.Delimiter_EOF);
            public static readonly Token OBlock = new Token("{", TokenType.Delimiter_OBlock);
            public static readonly Token OIndex = new Token("[", TokenType.Delimiter_OIndex);
            public static readonly Token OParenthesis = new Token("(", TokenType.Delimiter_OParenthesis);
            public static readonly Token StringAssigment = new Token('"'.ToString(), TokenType.Delimiter_StringAssigment);
            public static readonly Token TwoCollon = new Token("::", TokenType.Delimiter_TwoCollon);
        }

        public static class Directives
        {
            public static readonly Token Define = new Token("#define", TokenType.Directive_Define);
            public static readonly Token ElseIf = new Token("#elif", TokenType.Directive_ElseIf);
            public static readonly Token EndIf = new Token("#endif", TokenType.Directive_EndIf);
            public static readonly Token If = new Token("#if", TokenType.Directive_If);
            public static readonly Token IfDefined = new Token("#ifdef", TokenType.Directive_IfDefined);
            public static readonly Token IfNotDefine = new Token("#ifndef", TokenType.Directive_IfNotDefine);
            public static readonly Token Include = new Token("#include", TokenType.Directive_Include);
            public static readonly Token ThrowError = new Token("#error", TokenType.Directive_ThrowError);
            public static readonly Token UnDef = new Token("#undef", TokenType.Directive_UnDef);
        }

        public static class Keywords
        {
            public static readonly Token Asm = new Token("asm", TokenType.Keyword_Asm);
            public static readonly Token Auto = new Token("auto", TokenType.Keyword_Auto);
            public static readonly Token Base = new Token("base", TokenType.Keyword_Base);
            public static readonly Token Break = new Token("break", TokenType.Keyword_Break);
            public static readonly Token Case = new Token("case", TokenType.Keyword_Case);
            public static readonly Token Catch = new Token("catch", TokenType.Keyword_Catch);
            public static readonly Token Class = new Token("class", TokenType.Keyword_Class);
            public static readonly Token Constant = new Token("const", TokenType.Keyword_Constant);
            public static readonly Token Continue = new Token("continue", TokenType.Keyword_Continue);
            public static readonly Token Default = new Token("default", TokenType.Keyword_Default);
            public static readonly Token Do = new Token("do", TokenType.Keyword_Do);
            public static readonly Token Else = new Token("else", TokenType.Keyword_Else);
            public static readonly Token Enum = new Token("enum", TokenType.Keyword_Enum);
            public static readonly Token Extern = new Token("extern", TokenType.Keyword_Extern);
            public static readonly Token False = new Token("false", TokenType.Keyword_False);
            public static readonly Token For = new Token("for", TokenType.Keyword_For);
            public static readonly Token Global = new Token("global", TokenType.Keyword_Global);
            public static readonly Token If = new Token("if", TokenType.Keyword_If);
            public static readonly Token Interface = new Token("interface", TokenType.Keyword_Interface);
            public static readonly Token Namespace = new Token("namespace", TokenType.Keyword_Namespace);
            public static readonly Token New = new Token("new", TokenType.Keyword_New);
            public static readonly Token Null = new Token("null", TokenType.Keyword_Null);
            public static readonly Token Operator = new Token("operator", TokenType.Keyword_Operator);
            public static readonly Token Override = new Token("override", TokenType.Keyword_Override);
            public static readonly Token Readonly = new Token("readonly", TokenType.Keyword_Readonly);
            public static readonly Token Return = new Token("return", TokenType.Keyword_Return);
            public static readonly Token SizeOf = new Token("sizeof", TokenType.Keyword_SizeOf);
            public static readonly Token Static = new Token("static", TokenType.Keyword_Static);
            public static readonly Token Struct = new Token("struct", TokenType.Keyword_Struct);
            public static readonly Token Switch = new Token("switch", TokenType.Keyword_Switch);
            public static readonly Token This = new Token("this", TokenType.Keyword_This);
            public static readonly Token True = new Token("true", TokenType.Keyword_True);
            public static readonly Token Try = new Token("try", TokenType.Keyword_Try);
            public static readonly Token Using = new Token("using", TokenType.Keyword_Using);
            public static readonly Token Virtual = new Token("virtual", TokenType.Keyword_Virtual);
            public static readonly Token Volatile = new Token("volatile", TokenType.Keyword_Volatile);
            public static readonly Token While = new Token("while", TokenType.Keyword_While);
        }

        public static class Operators
        {
            public static readonly Token Add = new Token("+", TokenType.Operator_Add);
            public static readonly Token AddAssign = new Token("+=", TokenType.Operator_AddAssign);
            public static readonly Token And = new Token("&", TokenType.Operator_And);
            public static readonly Token Decrement = new Token("--", TokenType.Operator_Decrement);
            public static readonly Token Division = new Token("/", TokenType.Operator_Division);
            public static readonly Token Equal = new Token("=", TokenType.Operator_Equal);
            public static readonly Token Increment = new Token("++", TokenType.Operator_Increment);
            public static readonly Token Or = new Token("|", TokenType.Operator_Or);
            public static readonly Token Power = new Token("*", TokenType.Operator_Power);
            public static readonly Token Sub = new Token("-", TokenType.Operator_Sub);
            public static readonly Token SubAssign = new Token("-=", TokenType.Operator_SubAssign);
        }

        public static class BuiltInType
        {
            public static readonly Token Bool = new Token("bool", TokenType.BuiltInType_Bool);
            public static readonly Token Byte = new Token("byte", TokenType.BuiltInType_Byte);
            public static readonly Token Char = new Token("char", TokenType.BuiltInType_Char);
            public static readonly Token Double = new Token("double", TokenType.BuiltInType_Double);
            public static readonly Token Float = new Token("float", TokenType.BuiltInType_Float);
            public static readonly Token Int = new Token("int", TokenType.BuiltInType_Float);
            public static readonly Token UInt = new Token("uint", TokenType.BuiltInType_UInt);
            public static readonly Token Long = new Token("long", TokenType.BuiltInType_Long);
            public static readonly Token ULong = new Token("ulong", TokenType.BuiltInType_ULong);
            public static readonly Token String = new Token("string", TokenType.BuiltInType_String);
            public static readonly Token T = new Token("T", TokenType.BuiltInType_T);
            public static readonly Token Void = new Token("void", TokenType.BuiltInType_Void);
        }

        public static readonly Dictionary<string, Token> tokenDatabase = new Dictionary<string, Token>()
            {
                { AccessModifier.Private.Value, AccessModifier.Private },
                { AccessModifier.Protected.Value, AccessModifier.Protected },
                { AccessModifier.Public.Value, AccessModifier.Public },

                { Comparators.And.Value, Comparators.And },
                { Comparators.Different.Value, Comparators.Different },
                { Comparators.Equal.Value, Comparators.Equal },
                { Comparators.GreaterThan.Value, Comparators.GreaterThan },
                { Comparators.GreaterThanOrEqual.Value, Comparators.GreaterThanOrEqual },
                { Comparators.Inequality.Value, Comparators.Inequality },
                { Comparators.LessThan.Value, Comparators.LessThan },
                { Comparators.LessThanOrEqual.Value, Comparators.LessThanOrEqual },
                { Comparators.Or.Value, Comparators.Or },

                { Delimiters.Backslash.Value, Delimiters.Backslash },
                { Delimiters.CBlock.Value, Delimiters.CBlock },
                { Delimiters.CIndex.Value, Delimiters.CIndex },
                { Delimiters.CInstruction.Value, Delimiters.CInstruction },
                { Delimiters.Collon.Value, Delimiters.Collon },
                { Delimiters.Comma.Value, Delimiters.Comma },
                { Delimiters.CParenthesis.Value, Delimiters.CParenthesis },
                { Delimiters.Dot.Value, Delimiters.Dot },
                { Delimiters.OBlock.Value, Delimiters.OBlock },
                { Delimiters.OIndex.Value, Delimiters.OIndex },
                { Delimiters.OParenthesis.Value, Delimiters.OParenthesis },
                { Delimiters.TwoCollon.Value, Delimiters.TwoCollon },
                { Delimiters.EOF.Value, Delimiters.EOF },

                { Directives.Define.Value, Directives.Define },
                { Directives.ElseIf.Value, Directives.ElseIf },
                { Directives.EndIf.Value, Directives.EndIf },
                { Directives.If.Value, Directives.If },
                { Directives.IfDefined.Value, Directives.IfDefined },
                { Directives.IfNotDefine.Value, Directives.IfNotDefine },
                { Directives.Include.Value, Directives.Include },
                { Directives.ThrowError.Value, Directives.ThrowError },
                { Directives.UnDef.Value, Directives.UnDef },

                { Keywords.Asm.Value, Keywords.Asm },
                { Keywords.Auto.Value, Keywords.Auto },
                { Keywords.Base.Value, Keywords.Base },
                { Keywords.Break.Value, Keywords.Break },
                { Keywords.Case.Value, Keywords.Case },
                { Keywords.Catch.Value, Keywords.Catch },
                { Keywords.Class.Value, Keywords.Class },
                { Keywords.Constant.Value, Keywords.Constant },
                { Keywords.Continue.Value, Keywords.Continue },
                { Keywords.Default.Value, Keywords.Default },
                { Keywords.Do.Value, Keywords.Do },
                { Keywords.Else.Value, Keywords.Else },
                { Keywords.Enum.Value, Keywords.Enum },
                { Keywords.Extern.Value, Keywords.Extern },
                { Keywords.False.Value, Keywords.False },
                { Keywords.For.Value, Keywords.For },
                { Keywords.If.Value, Keywords.If },
                { Keywords.Interface.Value, Keywords.Interface},
                { Keywords.Namespace.Value, Keywords.Namespace },
                { Keywords.New.Value, Keywords.New },
                { Keywords.Null.Value, Keywords.Null },
                { Keywords.Operator.Value, Keywords.Operator },
                { Keywords.Override.Value, Keywords.Override },
                { Keywords.Readonly.Value, Keywords.Readonly },
                { Keywords.Return.Value, Keywords.Return },
                { Keywords.SizeOf.Value, Keywords.SizeOf },
                { Keywords.Static.Value, Keywords.Static },
                { Keywords.Struct.Value, Keywords.Struct },
                { Keywords.Switch.Value, Keywords.Switch},
                { Keywords.This.Value, Keywords.This },
                { Keywords.True.Value, Keywords.True },
                { Keywords.Try.Value, Keywords.Try },
                { Keywords.Using.Value, Keywords.Using },
                { Keywords.Virtual.Value, Keywords.Virtual },
                { Keywords.Volatile.Value, Keywords.Volatile },
                { Keywords.While.Value, Keywords.While  },

                { Operators.Add.Value, Operators.Add },
                { Operators.AddAssign.Value, Operators.AddAssign },
                { Operators.And.Value, Operators.And },
                { Operators.Decrement.Value, Operators.Decrement },
                { Operators.Division.Value, Operators.Division },
                { Operators.Equal.Value, Operators.Equal },
                { Operators.Increment.Value, Operators.Increment },
                { Operators.Or.Value, Operators.Or },
                { Operators.Power.Value, Operators.Power },
                { Operators.Sub.Value, Operators.Sub },
                { Operators.SubAssign.Value, Operators.SubAssign },

                { BuiltInType.Bool.Value, BuiltInType.Bool },
                { BuiltInType.Byte.Value, BuiltInType.Byte },
                { BuiltInType.Char.Value, BuiltInType.Char },
                { BuiltInType.Double.Value, BuiltInType.Double },
                { BuiltInType.Float.Value, BuiltInType.Float },
                { BuiltInType.Int.Value, BuiltInType.Int },
                { BuiltInType.UInt.Value, BuiltInType.UInt },
                { BuiltInType.Long.Value, BuiltInType.Long },
                { BuiltInType.ULong.Value, BuiltInType.ULong },
                { BuiltInType.String.Value, BuiltInType.String },
                { BuiltInType.T.Value, BuiltInType.T },
                { BuiltInType.Void.Value, BuiltInType.Void }
            };
    }
}
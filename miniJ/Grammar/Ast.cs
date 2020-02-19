using miniJ.Lexical.Elements.Token;

namespace miniJ.Grammar
{
    class AccessModifier
    {
        public static readonly Token Private = new Token("private") { TokenType = TokenType.AccessModifier_Private };
        public static readonly Token Protected = new Token("protected") { TokenType = TokenType.AccessModifier_Protected };
        public static readonly Token Public = new Token("public") { TokenType = TokenType.AccessModifier_Public };
    }

    class Comparators
    {
        public static readonly Token And = new Token("&&") { TokenType = TokenType.Comparator_And };
        public static readonly Token Different = new Token("!=") { TokenType = TokenType.Comparator_Different };
        public static readonly Token Equal = new Token("==") { TokenType = TokenType.Comparator_Equal };
        public static readonly Token GreaterThan = new Token(">") { TokenType = TokenType.Comparator_GreaterThan };
        public static readonly Token GreaterThanOrEqual = new Token(">=") { TokenType = TokenType.Comparator_GreaterThanOrEqual };
        public static readonly Token LessThan = new Token("<") { TokenType = TokenType.Comparator_LessThan };
        public static readonly Token LessThanOrEqual = new Token("<=") { TokenType = TokenType.Comparator_LessThanOrEqual };
        public static readonly Token Or = new Token("||") { TokenType = TokenType.Comparator_Or };
    }

    class Delimiters
    {
        public static readonly Token Backslash = new Token(@"\") { TokenType = TokenType.Delimiter_Backslash };
        public static readonly Token CBlock = new Token("}") { TokenType = TokenType.Delimiter_CBlock };
        public static readonly Token CharAssigment = new Token("'") { TokenType = TokenType.Delimiter_CharAssigment };
        public static readonly Token CIndex = new Token("]") { TokenType = TokenType.Delimiter_CIndex };
        public static readonly Token CInstruction = new Token(";") { TokenType = TokenType.Delimiter_CInstruction };
        public static readonly Token Collon = new Token(":") { TokenType = TokenType.Delimiter_Collon };
        public static readonly Token Comma = new Token(",") { TokenType = TokenType.Delimiter_Comma };
        public static readonly Token CParenthesis = new Token(")") { TokenType = TokenType.Delimiter_CParenthesis };
        public static readonly Token Dot = new Token(".") { TokenType = TokenType.Delimiter_Dot };
        public static readonly Token EOF = new Token("") { TokenType = TokenType.Delimiter_EOF };
        public static readonly Token OBlock = new Token("{") { TokenType = TokenType.Delimiter_OBlock };
        public static readonly Token OIndex = new Token("[") { TokenType = TokenType.Delimiter_OIndex };
        public static readonly Token OParenthesis = new Token("(") { TokenType = TokenType.Delimiter_OParenthesis };
        public static readonly Token StringAssigment = new Token('"'.ToString()) { TokenType = TokenType.Delimiter_StringAssigment };
        public static readonly Token TwoCollon = new Token("::") { TokenType = TokenType.Delimiter_TwoCollon };
    }

    class Directives
    {
        public static readonly Token Define = new Token("#define") { TokenType = TokenType.Directive_Define };
        public static readonly Token ElseIf = new Token("#elif") { TokenType = TokenType.Directive_ElseIf };
        public static readonly Token EndIf = new Token("#endif") { TokenType = TokenType.Directive_EndIf };
        public static readonly Token If = new Token("#if") { TokenType = TokenType.Directive_If };
        public static readonly Token IfDefined = new Token("#ifdef") { TokenType = TokenType.Directive_IfDefined };
        public static readonly Token IfNotDefine = new Token("#ifndef") { TokenType = TokenType.Directive_IfNotDefine };
        public static readonly Token Include = new Token("#include") { TokenType = TokenType.Directive_Include };
        public static readonly Token ThrowError = new Token("#error") { TokenType = TokenType.Directive_ThrowError };
        public static readonly Token UnDef = new Token("#undef") { TokenType = TokenType.Directive_UnDef };
    }

    class Keywords
    {
        public static readonly Token Asm = new Token("asm") { TokenType = TokenType.Keyword_Asm };
        public static readonly Token Auto = new Token("auto") { TokenType = TokenType.Keyword_Auto };
        public static readonly Token Base = new Token("base") { TokenType = TokenType.Keyword_Base };
        public static readonly Token Break = new Token("break") { TokenType = TokenType.Keyword_Break };
        public static readonly Token Case = new Token("case") { TokenType = TokenType.Keyword_Case };
        public static readonly Token Catch = new Token("catch") { TokenType = TokenType.Keyword_Catch };
        public static readonly Token Class = new Token("class") { TokenType = TokenType.Keyword_Class };
        public static readonly Token Constant = new Token("const") { TokenType = TokenType.Keyword_Constant };
        public static readonly Token Continue = new Token("continue") { TokenType = TokenType.Keyword_Continue };
        public static readonly Token Default = new Token("default") { TokenType = TokenType.Keyword_Default };
        public static readonly Token Do = new Token("do") { TokenType = TokenType.Keyword_Do };
        public static readonly Token Else = new Token("else") { TokenType = TokenType.Keyword_Else };
        public static readonly Token Enum = new Token("enum") { TokenType = TokenType.Keyword_Enum };
        public static readonly Token Extern = new Token("extern") { TokenType = TokenType.Keyword_Extern };
        public static readonly Token False = new Token("false") { TokenType = TokenType.Keyword_False };
        public static readonly Token For = new Token("for") { TokenType = TokenType.Keyword_For };
        public static readonly Token Global = new Token("global") { TokenType = TokenType.Keyword_Global };
        public static readonly Token If = new Token("if") { TokenType = TokenType.Keyword_If };
        public static readonly Token Import = new Token("import") { TokenType = TokenType.Keyword_Import };
        public static readonly Token Interface = new Token("interface") { TokenType = TokenType.Keyword_Interface };
        public static readonly Token Namespace = new Token("namespace") { TokenType = TokenType.Keyword_Namespace };
        public static readonly Token New = new Token("new") { TokenType = TokenType.Keyword_New };
        public static readonly Token Null = new Token("null") { TokenType = TokenType.Keyword_Null };
        public static readonly Token Readonly = new Token("readonly") { TokenType = TokenType.Keyword_Readonly };
        public static readonly Token Return = new Token("return") { TokenType = TokenType.Keyword_Return };
        public static readonly Token Signed = new Token("signed") { TokenType = TokenType.Keyword_Signed };
        public static readonly Token SizeOf = new Token("sizeof") { TokenType = TokenType.Keyword_SizeOf };
        public static readonly Token Struct = new Token("struct") { TokenType = TokenType.Keyword_Struct };
        public static readonly Token Switch = new Token("switch") { TokenType = TokenType.Keyword_Switch };
        public static readonly Token This = new Token("this") { TokenType = TokenType.Keyword_This };
        public static readonly Token True = new Token("true") { TokenType = TokenType.Keyword_True };
        public static readonly Token Try = new Token("try") { TokenType = TokenType.Keyword_Try };
        public static readonly Token Unsigned = new Token("unsigned") { TokenType = TokenType.Keyword_Unsigned };
        public static readonly Token Volatile = new Token("volatile") { TokenType = TokenType.Keyword_Volatile };
        public static readonly Token While = new Token("while") { TokenType = TokenType.Keyword_While };
        //   public static readonly Token Register = new Token("register"; // The register keyword creates register variables which are much faster than normal variables.
        //   public static readonly Token Onion = new Token("union"; // A Union is used for grouping different types of variable under a single name and allocates memory only of the biggest variable.
    }

    class Operators
    {
        public static readonly Token Add = new Token("+") { TokenType = TokenType.Operator_Add };
        public static readonly Token AddAssign = new Token("+=") { TokenType = TokenType.Operator_AddAssign };
        public static readonly Token And = new Token("&") { TokenType = TokenType.Operator_And };
        public static readonly Token Decrement = new Token("--") { TokenType = TokenType.Operator_Decrement };
        public static readonly Token Division = new Token("/") { TokenType = TokenType.Operator_Division };
        public static readonly Token Equal = new Token("=") { TokenType = TokenType.Operator_Equal };
        public static readonly Token Increment = new Token("++") { TokenType = TokenType.Operator_Increment };
        public static readonly Token Or = new Token("|") { TokenType = TokenType.Operator_Or };
        public static readonly Token Power = new Token("*") { TokenType = TokenType.Operator_Power };
        public static readonly Token Sub = new Token("-") { TokenType = TokenType.Operator_Sub };
        public static readonly Token SubAssign = new Token("-=") { TokenType = TokenType.Operator_SubAssign };
    }

    class Types
    {
        public static readonly Token Bool = new Token("bool") { TokenType = TokenType.PrimitiveType_Bool };
        public static readonly Token Byte = new Token("byte") { TokenType = TokenType.PrimitiveType_Byte };
        public static readonly Token Char = new Token("char") { TokenType = TokenType.PrimitiveType_Char };
        public static readonly Token Double = new Token("double") { TokenType = TokenType.PrimitiveType_Double };
        public static readonly Token Float = new Token("float") { TokenType = TokenType.PrimitiveType_Float };
        public static readonly Token Int = new Token("int") { TokenType = TokenType.PrimitiveType_Int };
        public static readonly Token Long = new Token("long") { TokenType = TokenType.PrimitiveType_Long };
        public static readonly Token String = new Token("string") { TokenType = TokenType.PrimitiveType_String };
        public static readonly Token Void = new Token("void") { TokenType = TokenType.PrimitiveType_Void };
    }
}
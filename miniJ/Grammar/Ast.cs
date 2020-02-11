using miniJ.Lexical.Elements.Token;

namespace miniJ.Grammar
{
    class Keywords
    {
        public static readonly LexerToken Break = new LexerToken("break") { Type = TokenType.Keyword };
        public static readonly LexerToken Import = new LexerToken("import") { Type = TokenType.Keyword };
        public static readonly LexerToken Continue = new LexerToken("continue") { Type = TokenType.Keyword };
        public static readonly LexerToken If = new LexerToken("if") { Type = TokenType.Keyword };
        public static readonly LexerToken Else = new LexerToken("else") { Type = TokenType.Keyword };
        public static readonly LexerToken While = new LexerToken("while") { Type = TokenType.Keyword };
        public static readonly LexerToken For = new LexerToken("for") { Type = TokenType.Keyword };
        public static readonly LexerToken Return = new LexerToken("return") { Type = TokenType.Keyword };
        public static readonly LexerToken Constant = new LexerToken("const") { Type = TokenType.Keyword };
        public static readonly LexerToken True = new LexerToken("true") { Type = TokenType.Keyword };
        public static readonly LexerToken False = new LexerToken("false") { Type = TokenType.Keyword };
        public static readonly LexerToken Null = new LexerToken("null") { Type = TokenType.Keyword };
        public static readonly LexerToken Class = new LexerToken("class") { Type = TokenType.Keyword };
        public static readonly LexerToken Interface = new LexerToken("interface") { Type = TokenType.Keyword };
        public static readonly LexerToken Struct = new LexerToken("struct") { Type = TokenType.Keyword };
        public static readonly LexerToken Namespace = new LexerToken("namespace") { Type = TokenType.Keyword };
        public static readonly LexerToken Enum = new LexerToken("enum") { Type = TokenType.Keyword };
        public static readonly LexerToken New = new LexerToken("new") { Type = TokenType.Keyword };
        public static readonly LexerToken Base = new LexerToken("base") { Type = TokenType.Keyword };
        public static readonly LexerToken Signed = new LexerToken("signed") { Type = TokenType.Keyword };
        public static readonly LexerToken Switch = new LexerToken("switch") { Type = TokenType.Keyword };
        public static readonly LexerToken Case = new LexerToken("case") { Type = TokenType.Keyword };
        public static readonly LexerToken Unsigned = new LexerToken("unsigned") { Type = TokenType.Keyword };
        public static readonly LexerToken This = new LexerToken("this") { Type = TokenType.Keyword };
        public static readonly LexerToken Auto = new LexerToken("auto") { Type = TokenType.Keyword };
        public static readonly LexerToken Asm = new LexerToken("asm") { Type = TokenType.Keyword };
        public static readonly LexerToken Try = new LexerToken("try") { Type = TokenType.Keyword };
        public static readonly LexerToken Catch = new LexerToken("catch") { Type = TokenType.Keyword };
        public static readonly LexerToken Default = new LexerToken("default") { Type = TokenType.Keyword };
        public static readonly LexerToken Do = new LexerToken("do") { Type = TokenType.Keyword };
        public static readonly LexerToken Extern = new LexerToken("extern") { Type = TokenType.Keyword };
        public static readonly LexerToken SizeOf = new LexerToken("sizeof") { Type = TokenType.Keyword };
        public static readonly LexerToken Volatile = new LexerToken("volatile") { Type = TokenType.Keyword };
        public static readonly LexerToken Readonly = new LexerToken("readonly") { Type = TokenType.Keyword };
        //   public static readonly Token Register = new Token("register"; // The register keyword creates register variables which are much faster than normal variables.
        //   public static readonly Token Onion = new Token("union"; // A Union is used for grouping different types of variable under a single name and allocates memory only of the biggest variable.
    }

    class AccessModifier
    {
        public static readonly LexerToken Public = new LexerToken("public") { Type = TokenType.AccessModifier };
        public static readonly LexerToken Private = new LexerToken("private") { Type = TokenType.AccessModifier };
        public static readonly LexerToken Protected = new LexerToken("protected") { Type = TokenType.AccessModifier };
    }

    class Delimiters
    {
        public static readonly LexerToken OBlock = new LexerToken("{") { Type = TokenType.Delimiter };
        public static readonly LexerToken CBlock = new LexerToken("}") { Type = TokenType.Delimiter };
        public static readonly LexerToken Dot = new LexerToken(".") { Type = TokenType.Delimiter };
        public static readonly LexerToken OParenthesis = new LexerToken("(") { Type = TokenType.Delimiter };
        public static readonly LexerToken CParenthesis = new LexerToken(")") { Type = TokenType.Delimiter };
        public static readonly LexerToken OIndex = new LexerToken("[") { Type = TokenType.Delimiter };
        public static readonly LexerToken CIndex = new LexerToken("]") { Type = TokenType.Delimiter };
        public static readonly LexerToken Comma = new LexerToken(",") { Type = TokenType.Delimiter };
        public static readonly LexerToken CInstruction = new LexerToken(";") { Type = TokenType.Delimiter };
        public static readonly LexerToken Collon = new LexerToken(":") { Type = TokenType.Delimiter };
        public static readonly LexerToken Backslash = new LexerToken(@"\") { Type = TokenType.Delimiter };
    }

    class Comparators
    {
        public static readonly LexerToken LessThan = new LexerToken("<") { Type = TokenType.Comparator };
        public static readonly LexerToken GreaterThan = new LexerToken(">") { Type = TokenType.Comparator };
        public static readonly LexerToken Different = new LexerToken("!=") { Type = TokenType.Comparator };
        public static readonly LexerToken Equal = new LexerToken("==") { Type = TokenType.Comparator };
        public static readonly LexerToken And = new LexerToken("&&") { Type = TokenType.Comparator };
        public static readonly LexerToken Or = new LexerToken("||") { Type = TokenType.Comparator };
    }

    class Operators
    {
        public static readonly LexerToken Add = new LexerToken("+") { Type = TokenType.Operator };
        public static readonly LexerToken Sub = new LexerToken("-") { Type = TokenType.Operator };
        public static readonly LexerToken Division = new LexerToken("/") { Type = TokenType.Operator };
        public static readonly LexerToken And = new LexerToken("&") { Type = TokenType.Operator };
        public static readonly LexerToken Or = new LexerToken("|") { Type = TokenType.Operator };
        public static readonly LexerToken Power = new LexerToken("*") { Type = TokenType.Operator };
        public static readonly LexerToken Equal = new LexerToken("=") { Type = TokenType.Operator };
        public static readonly LexerToken SubAssign = new LexerToken("-=") { Type = TokenType.Operator };
        public static readonly LexerToken AddAssign = new LexerToken("+=") { Type = TokenType.Operator };
        public static readonly LexerToken Increment = new LexerToken("++") { Type = TokenType.Operator };
        public static readonly LexerToken Decrement = new LexerToken("--") { Type = TokenType.Operator };
    }

    class Types
    {
        public static readonly LexerToken Byte = new LexerToken("byte") { Type = TokenType.PrimitiveType };
        public static readonly LexerToken Char = new LexerToken("char") { Type = TokenType.PrimitiveType };
        public static readonly LexerToken Int = new LexerToken("int") { Type = TokenType.PrimitiveType };
        public static readonly LexerToken Long = new LexerToken("long") { Type = TokenType.PrimitiveType };
        public static readonly LexerToken String = new LexerToken("string") { Type = TokenType.PrimitiveType };
        public static readonly LexerToken Double = new LexerToken("double") { Type = TokenType.PrimitiveType };
        public static readonly LexerToken Float = new LexerToken("float") { Type = TokenType.PrimitiveType };
        public static readonly LexerToken Void = new LexerToken("void") { Type = TokenType.PrimitiveType };
        public static readonly LexerToken Bool = new LexerToken("bool") { Type = TokenType.PrimitiveType };
    }

    class Directives
    {
        public static readonly LexerToken Define = new LexerToken("#define") { Type = TokenType.Directive };
        public static readonly LexerToken UnDef = new LexerToken("#undef") { Type = TokenType.Directive };
        public static readonly LexerToken If = new LexerToken("#if") { Type = TokenType.Directive };
        public static readonly LexerToken ElseIf = new LexerToken("#elif") { Type = TokenType.Directive };
        public static readonly LexerToken IfDefined = new LexerToken("#ifdef") { Type = TokenType.Directive };
        public static readonly LexerToken IfNotDefine = new LexerToken("#ifndef") { Type = TokenType.Directive };
        public static readonly LexerToken EndIf = new LexerToken("#endif") { Type = TokenType.Directive };
        public static readonly LexerToken ThrowError = new LexerToken("#error") { Type = TokenType.Directive };
        public static readonly LexerToken Include = new LexerToken("#include") { Type = TokenType.Directive };
    }
}
namespace miniJ.Lexical.Elements.Token
{
    public enum TokenType
    {
        None,
        Identifier,
        Number,
        String,
        Char,

        // PREDEFINIDO \\
        Keyword,

        Symbol,
        Operator,
        Comparator,
        Delimiter,
        PrimitiveType,
        AccessModifier,
        Directive

        // PREDEFINIDO \\
    }
}
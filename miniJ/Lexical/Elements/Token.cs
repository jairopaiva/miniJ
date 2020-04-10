namespace miniJ.Lexical.Elements.Token
{
    public class Token
    {
        public Token(string value, TokenType tokenType = TokenType.NotDef_None)
        {
            Value = value;
            TokenType = tokenType;
        }

        public NodeLocation Location;
        public TokenType TokenType;
        public readonly string Value;

        public string ToString(bool location = false)
        {
            string info = "Token[" + this.Location.Line + ":" + this.Location.Column + "] = " + this.TokenType + " - Value = " + this.Value;
            if (location)
                info = this.Location.File + " :: " + info;
            return info;
        }

        public bool IsToken(string type)
        {
            return this.TokenType.ToString().StartsWith(type);
        }

        public const string ACCESS_MODIFIER = "AccessModifier";
        public const string COMPARATOR = "Comparator";
        public const string DELIMITER = "Delimiter";
        public const string DIRECTIVE = "Directive";
        public const string KEYWORD = "Keyword";
        public const string OPERATOR = "Operator";
        public const string NOT_DEF = "NotDef";
        public const string BuiltInType = "BuiltInType";
    }
}
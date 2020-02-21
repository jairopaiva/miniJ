namespace miniJ.Lexical.Elements.Token
{
    class Token
    {
        public Token(string value)
        {
            Value = value;
            TokenType = TokenType.NotDef_None;
        }

        public NodeLocation Location { get; set; }
        public TokenType TokenType { get; set; }
        public string Value { get; set; }

        public Token Copy(NodeLocation location)
        {
            Token clone = new Token(this.Value) { TokenType = this.TokenType, Location = location };
            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Token))
                return (obj as Token).Value == this.Value;
            return obj.ToString() == this.Value;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode() ^ this.TokenType.GetHashCode();
        }

        public string ToString(bool location = false)
        {
            string info = "Token[" + this.Location.Line + ":" + this.Location.Column + "] = " + this.TokenType + " - Value = " + this.Value;
            if (location)
                info = this.Location.File + " :: " + info;
            return info;
        }
    }
}
namespace miniJ.Lexical.Elements.Token
{
     class LexerToken : LexerNode
    {
        public string Value { get; set; }
        public TokenType Type { get; set; }

        public LexerToken(string value)
        {
            Value = value;
            Type = TokenType.None;
        }

        public LexerToken Copy(TokenLocation location)
        {
            LexerToken clone = new LexerToken(this.Value) { Type = this.Type, Location = location };
            return clone;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(LexerToken))
                return (obj as LexerToken).Value == this.Value;
            return obj.ToString() == this.Value;
        }

        public override string ToString()
        {
            return "Token[" + this.Location.Line + ":" + this.Location.Column + "] = " + this.Value + " : " + this.Type;
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode() ^ this.Type.GetHashCode();
        }

    }
}
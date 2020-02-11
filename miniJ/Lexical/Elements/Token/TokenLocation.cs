namespace miniJ.Lexical.Elements.Token
{
    struct TokenLocation
    {
        public int Column { get; set; }
        public int Line { get; set; }
        public string File { get; set; }
    }
}
namespace miniJ.Lexical.Elements.Token
{
     struct NodeLocation
    {
        public int Column { get; set; }
        public string File { get; set; }
        public int Line { get; set; }

        public override string ToString()
        {
            return "Line: " + Line + " - Column: " + Column;
        }
    }
}
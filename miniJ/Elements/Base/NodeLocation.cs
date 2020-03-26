namespace miniJ.Lexical.Elements.Token
{
     struct NodeLocation
    {
        public int Column;
        public string File;
        public int Line;

        public override string ToString()
        {
            return "Line: " + Line + " - Column: " + Column;
        }
    }
}
namespace miniJ.Lexical.Elements.Token
{
    public struct NodeLocation
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
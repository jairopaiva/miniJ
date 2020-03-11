using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class ParameterDeclaration : Variable
    {
        public ParameterDeclaration(Token origin) : base(origin)
        {
        }
    }
}
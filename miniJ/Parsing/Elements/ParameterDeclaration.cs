using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements.Symbols;

namespace miniJ.Parsing.Elements
{
    public class ParameterDeclaration : Variable
    {
        public ParameterDeclaration(Token origin) : base(origin)
        {
        }
    }
}
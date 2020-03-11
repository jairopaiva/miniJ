using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;

namespace miniJ.Parsing.Elements
{
    class Method : ISyntaxNode
    {
        public DataType CISE { get; set; }
        public List<ParameterDeclaration> Parameters { get; set; }
        public Variable ReturnValue { get; set; }

        public Method(Token origin) : base(origin)
        {
        }
    }
}
using miniJ.Lexical.Elements.Token;
namespace miniJ.Elements.Base.Error
{
    class ParserError : CompilationError
    {
        public ParserError(ICompilerNode compilerNode, Token errorToken, string errorMessage) : base(compilerNode, errorMessage, errorToken.Value)
        {
            Location = errorToken.Location.ToString();
        }
    }
}

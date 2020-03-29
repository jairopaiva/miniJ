using miniJ.Lexical.Elements.Token;

namespace miniJ.Elements.Base
{
    class CodeError
    {
        public Token ErrorToken;
        public ICompilerNode CompilerNode;
        public string ErrorMessage;
        public CodeError(Token errorToken, ICompilerNode compilerNode, string errorMessage)
        {
            ErrorToken = errorToken;
            CompilerNode = compilerNode;
            ErrorMessage = errorMessage;
        }

    }
}

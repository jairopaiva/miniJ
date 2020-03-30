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

        public const string INVALID_MEMBER_MODIFIER = "Cannot use this member modifier in parameter declaration.";
        public const string INVALID_TOKEN = "Invalid token!";
        public const string NAMESPACE_ALREADY_CLOSED = "The namespace has already been closed: ";

    }
}

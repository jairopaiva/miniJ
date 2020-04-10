namespace miniJ.Elements.Base.Error
{
    class LexerError : CompilationError
    {
        public LexerError(ICompilerNode compilerNode, string errorToken, string errorLocation, string errorMessage) : base(compilerNode, errorMessage, errorToken)
        {
            Location = errorLocation;
        }
    }
}

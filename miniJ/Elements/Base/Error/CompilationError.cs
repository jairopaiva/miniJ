namespace miniJ.Elements.Base.Error
{
    public class CompilationError
    {
        public ICompilerNode CompilerNode;
        public string Location;
        public string ErrorMessage;
        public string ErrorToken;
        public CompilationError(ICompilerNode compilerNode, string errorMessage, string errorToken)
        {
            CompilerNode = compilerNode;
            ErrorMessage = errorMessage;
            ErrorToken = errorToken;
        }

        public override string ToString()
        {
            return "[" + CompilerNode.GetType().Name + "]: '" + ErrorMessage + "', at " + Location + ", value = '"+ErrorToken+"'";
        }
    }
}

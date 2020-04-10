using static miniJ.Elements.Base.Grammar;
using System.Collections.Generic;
using miniJ.Elements.Base.Error;

namespace miniJ.Elements.Base.CompilationElements
{
    public class CompilationUnit
    {
        public readonly List<CompilationError> Errors;
        public readonly Namespace GlobalNamespace;
        public readonly LexerResult LexerResult;
        public CompilationUnit()
        {
            LexerResult = new LexerResult();
            GlobalNamespace = new Namespace(Keywords.Global, null)
            {
                Name = Keywords.Global.Value
            };
            Errors = new List<CompilationError>();
        }
    }
}

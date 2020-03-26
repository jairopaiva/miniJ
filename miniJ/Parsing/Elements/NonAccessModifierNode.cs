using miniJ.Elements.Base;
using miniJ.Grammar;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class NonAccessModifierNode : ISyntaxNode
    {
        public SpecificNonAccessModifier SpecificNonAccessModifier;
        private NonAccessModifierNode(Token origin, SpecificNonAccessModifier specificAccessModifier) : base(origin)
        { 
            SpecificNonAccessModifier = specificAccessModifier; 
        }

        public static NonAccessModifierNode Get(Token origin, SpecificNonAccessModifier accessModifier)
        {
            return new NonAccessModifierNode(origin, accessModifier);
        }

        public static NonAccessModifierNode Get(Token origin)
        {
            return new NonAccessModifierNode(origin, GetSpecificNonAccessModifier(origin));
        }

        public static SpecificNonAccessModifier GetSpecificNonAccessModifier(Token origin)
        {
            switch (origin.TokenType)
            {
                case TokenType.Keyword_Constant:
                    return SpecificNonAccessModifier.Constant;
                case TokenType.Keyword_Override:
                    return SpecificNonAccessModifier.Override;
                case TokenType.Keyword_Readonly:
                    return SpecificNonAccessModifier.Readonly;
                case TokenType.Keyword_Volatile:
                    return SpecificNonAccessModifier.Volatile;
                default:
                    throw new System.Exception();
            }
        }

        public override string ToString()
        {
            return this.GetType().Name + " of type " + SpecificNonAccessModifier;
        }
    }

    public enum SpecificNonAccessModifier
    {
        Constant,
        Override,
        Readonly,
        Volatile
    }
}
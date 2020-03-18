using miniJ.Elements.Base;
using miniJ.Grammar;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class AccessModifierNode : ISyntaxNode
    {
        public SpecificAccessModifier SpecificAccessModifier;
        private AccessModifierNode(Token origin, SpecificAccessModifier specificAccessModifier) : base(origin)
        { SpecificAccessModifier = specificAccessModifier; }

        public static AccessModifierNode Get(Token origin, SpecificAccessModifier accessModifier)
        {
            return new AccessModifierNode(origin, accessModifier);
        }

        public static AccessModifierNode Get(Token origin)
        {
            return new AccessModifierNode(origin, GetSpecificAccessModifier(origin));
        }

        public static SpecificAccessModifier GetSpecificAccessModifier(Token origin)
        {
            switch (origin.TokenType)
            {
                case TokenType.AccessModifier_Private:
                    return SpecificAccessModifier.Private;
                case TokenType.AccessModifier_Protected:
                    return SpecificAccessModifier.Protected;
                case TokenType.AccessModifier_Public:
                    return SpecificAccessModifier.Public;
                default:
                    throw new System.Exception();
            }
        }

        public override string ToString()
        {
            return this.GetType().Name + " of type " + SpecificAccessModifier;
        }
    }

    public enum SpecificAccessModifier
    {
        Public,
        Private,
        Protected
    }
}
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;

namespace miniJ.Parsing.Elements
{
    class AccessModifierNode : ISyntaxNode
    {
        public SpecificAccessModifier SpecificAccessModifier;
        private AccessModifierNode(Token origin, SpecificAccessModifier specificAccessModifier) : base(origin)
        {
            SpecificAccessModifier = specificAccessModifier; 
        }

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
                    return SpecificAccessModifier.PRIVATE;
                case TokenType.AccessModifier_Protected:
                    return SpecificAccessModifier.PROTECTED;
                case TokenType.AccessModifier_Public:
                    return SpecificAccessModifier.PUBLIC;
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
        PROTECTED = 0x0001,
        PUBLIC = 0x0002,
        PRIVATE = 0x0004
    }
}
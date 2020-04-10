
using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements.Symbols;
using System.Collections.Generic;

namespace miniJ.Parsing.Elements
{
    public class CISE : SyntaxNode
    {
        /// <summary>
        /// Escopo de acesso para este CISE
        /// </summary>
        public AccessModifierNode AccessModifier;
        public SpecificTypeOfCISE TypeOfCISE;
        public List<Method> Methods;
        public Method Constructor;
        /// <summary>
        /// CISES que foram declarados dentro deste
        /// </summary>
        public List<CISE> Children;
        /// <summary>
        /// Namespace em que este CISE está declarado
        /// </summary>
        public Namespace Namespace;        
        public List<Field> Fields;
        public string Name;
        /// <summary>
        /// Quando diferente de null, quer dizer que este CISE foi declarado dentro de outro
        /// </summary>
        public CISE Root;
       
        public CISE(Token name, SpecificTypeOfCISE type, Token origin) : base(origin)
        {
            Name = name.Value;
            TypeOfCISE = type;
            Fields = new List<Field>();
            Children = new List<CISE>();
            Methods = new List<Method>();
        }

        public enum SpecificTypeOfCISE
        {
            Class,
            Interface,
            Struct,
            Enum
        }

        public override string ToString()
        {
            string insideOf = string.Empty;
            if (Root != null)
            {
                insideOf = Root.ToString();
            }
            else
            {
                insideOf= Namespace.ToString();
            }
            return TypeOfCISE + ", " +
                Name + ", inside " + insideOf;
        }

    }
}
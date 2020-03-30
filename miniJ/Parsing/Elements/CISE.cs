
using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System;
using System.Collections.Generic;

namespace miniJ.Parsing.Elements
{
    class CISE : ISyntaxNode
    {

        
        /// <summary>
        /// Escopo de acesso para este CISE
        /// </summary>
        public AccessModifierNode AccessModifier;
        public SpecificTypeOfCISE TypeOfCISE;
        public AbstractMethod Constructor;
        /// <summary>
        /// CISES que foram declarados dentro deste
        /// </summary>
        public List<CISE> Children;
        /// <summary>
        /// Namespace em que este CISE está declarado
        /// </summary>
        public Namespace Namespace;
        public string Name;
        /// <summary>
        /// Quando diferente de null, quer dizer que este CISE foi declarado dentro de outro
        /// </summary>
        public CISE Root;
       
        public CISE(Token name, SpecificTypeOfCISE type, Token origin) : base(origin)
        {
            Name = name.Value;
            TypeOfCISE = type;
            Children = new List<CISE>();
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
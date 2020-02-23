﻿using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;

namespace miniJ.Elements
{
    class Namespace : ISyntaxNode
    {
        public readonly Namespace Parent;

        public Namespace(Token origin, Namespace parent) : base(origin)
        {
            Name = origin.Value;
            Childs = new List<Namespace>();
            CISEs = new Dictionary<string, DataType>();
            Imports = new List<Namespace>();
            Parent = parent;
        }

        public List<Namespace> Childs { get; set; }
        public Dictionary<string, DataType> CISEs { get; set; }
        public List<Namespace> Imports { get; set; }
        public string Name { get; set; } // Nome específico

        public Namespace Clone()
        {
            Namespace clone = new Namespace(this.Origin, this.Parent)
            {
                Childs = this.Childs,
                CISEs = this.CISEs,
                Imports = this.Imports
            };
            return clone;
        }
    }
}
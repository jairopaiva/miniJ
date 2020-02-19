using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;

namespace miniJ.Elements
{
    class Namespace : ISyntaxNode
    {
        public List<Namespace> Path { get; set; }
        public Dictionary<string, CISE> CISEs { get; set; }
        public List<Namespace> Imports { get; set; }
        public readonly Namespace Parent;
        public string Name { get; set; } // Nome específico

        public Namespace(Token origin, Namespace parent) : base(origin)
        {
            Path = new List<Namespace>();
            CISEs = new Dictionary<string, CISE>();
            Imports = new List<Namespace>();
            Parent = parent;
        }
    }

    /*
    class RootNamespace : Namespace
    {
        public RootNamespace() : base(null, null)
        {
            AllCISEs = new Dictionary<string, CISE>();
            AllNamespaces = new Dictionary<string, Namespace>();
        }

        public Dictionary<string, Namespace> AllNamespaces { get; set; }
        public Dictionary<string, CISE> AllCISEs { get; set; }
    }*/
}
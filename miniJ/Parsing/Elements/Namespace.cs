using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;
using System.Text;

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

        public override string ToString()
        {
            StringBuilder namespaceCompletePath = new StringBuilder();
            List<string> namespaceNames = new List<string>();
            Namespace n = this;
            while (n != null)
            {
                string name = n.Name;
                namespaceNames.Add(name);
                n = n.Parent;
            }
            namespaceNames.Reverse();
            int limit = namespaceNames.Count - 1;
            for (int i = 0; i < namespaceNames.Count; i++)
            {
                namespaceCompletePath.Append(namespaceNames[i]);
                if (i != limit)
                    namespaceCompletePath.Append(".");
            }
            return namespaceCompletePath.ToString();
        }
    }
}
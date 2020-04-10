using miniJ.Elements.Base;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Elements
{
    public class Namespace : SyntaxNode
    {
        public readonly Namespace Parent;

        public Namespace(Token origin, Namespace parent) : base(origin)
        {
            Childs = new List<Namespace>();
            CISEs = new Dictionary<string, CISE>();
            Imports = new List<Namespace>();
            Parent = parent;
            Open = true;
        }

        public List<Namespace> Childs;
        public Dictionary<string, CISE> CISEs;
        public List<Namespace> Imports;
        public string Name; // Nome específico
        public bool Open; // Se true, ainda está sendo processado, caso false, já foi processado e "fechado(})"

        public Namespace Clone()
        {
            Namespace clone = new Namespace(this.Origin, this.Parent)
            {
                Name = this.Name,
                Childs = this.Childs,
                CISEs = this.CISEs,
                Imports = this.Imports,
                Open = this.Open
            };
            return clone;
        }

        public bool CanImportNamespace(List<Token> path)
        {
            return true;
        }

        private List<string> GetInverseCompletePath()
        {
            List<string> namespaceNames = new List<string>();
            Namespace n = this;
            while (n != null)
            {
                string name = n.Name;
                namespaceNames.Add(name);
                n = n.Parent;
            }
            return namespaceNames;
        }
        public override string ToString()
        {
            StringBuilder namespaceCompletePath = new StringBuilder();
            List<string> namespaceNames = GetInverseCompletePath();
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
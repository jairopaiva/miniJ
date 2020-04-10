using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;

namespace miniJ.Parsing.Elements.Symbols
{
    /// <summary>
    /// Inside CISE, out method
    /// </summary>
    public class Field : Variable
    {
        public AccessModifierNode AccessModifier;
        public Field(Token origin) : base(origin)
        {
        }

        public static List<Field> Convert(List<Variable> variables, AccessModifierNode accessModifier)
        {
            List<Field> fields = new List<Field>();
            for (int i = 0; i < variables.Count; i++)
            {
                Variable Var = variables[i];
                Field field = new Field(Var.Origin)
                {
                    AccessModifier = accessModifier,
                    Name = Var.Name,
                    Type = Var.Type,
                    Value = Var.Value
                };
                fields.Add(field);
            }
            return fields;
        }
    }
}

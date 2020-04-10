using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Elements.Base.CompilationElements;
using miniJ.Parsing.Elements;
using miniJ.Parsing.Elements.DataTypes;
using miniJ.Parsing.Elements.Symbols;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Helpers
{
    /// <summary>
    /// Recria o código com base no que foi processado, muito bom para se detectar erros,
    /// pois, se não se pode ser recriado, o compilador não o entendeu completamente.
    /// </summary>
    static class Remaker
    {

        private static StringBuilder builder;
        public static string Recreate(CompilationUnit compilationUnit)
        {
            builder = new StringBuilder();
            tabTimes = 0;
            NamespaceDclrToString(compilationUnit.GlobalNamespace);
            return builder.ToString();
        }

        private static void NamespaceDclrToString(Namespace Namespace)
        {
            builder.Append(Grammar.Keywords.Namespace.Value);
            Space();
            builder.Append(Namespace.Name);
            
            if (Namespace.CISEs.Count > 0)
            {
                NamespaceBodyToString(Namespace);
            }

            foreach (Namespace @namespace in Namespace.Childs)
            {
                NamespaceDclrToString(@namespace);
            }
        }

        private static void NamespaceBodyToString(Namespace Namespace)
        {
            OBlock();
            foreach (CISE cise in Namespace.CISEs.Values)
            {
                CISEToString(cise);
            }
            CBlock();
        }

        private static void CISEToString(CISE cise)
        {
            builder.Append(cise.Origin.Value);
            Space();
            builder.Append(cise.Name);

            OBlock();
            FieldsToString(cise.Fields);

            if (cise.Constructor != null)
            {
                MethodToString(cise.Constructor);
                NewLine();
            }

            foreach (Method method in cise.Methods)
            {
                MethodToString(method);
            }

            if (cise.Children.Count > 0)
            {
                foreach (CISE children in cise.Children)
                {
                    CISEToString(children);
                }
            }

            CBlock();
        }

        private static void MethodToString(Method method)
        {
            builder.Append(method.AccessModifier.Origin.Value);
            Space();

            if (method.Virtual)
            {
                builder.Append(Grammar.Keywords.Virtual.Value);
                Space();
            }

            DataTypeToString(method.ReturnType);
            Space();
            builder.Append(method.Name);
            Space();
            ParametersToString(method.Parameters);

            // Quando o parser estiver completo, implemento essa parte...
            builder.Append(Grammar.Delimiters.CInstruction.Value);
            NewLine();
        }

        private static void ParametersToString(List<ParameterDeclaration> parameters)
        {
            builder.Append(Grammar.Delimiters.OParenthesis.Value);
            for (int i = 0; i < parameters.Count; i++)
            {
                ParameterDeclaration parameter = parameters[i];
                DataTypeToString(parameter.Type);
                Space();
                builder.Append(parameter.Name);
                if (i != parameters.Count - 1)
                {
                    builder.Append(Grammar.Delimiters.Comma.Value);
                    Space();
                }
            }
            builder.Append(Grammar.Delimiters.CParenthesis.Value);
        }

        private static void FieldsToString(List<Field> fields)
        {
            foreach (Field field in fields)
            {
                builder.Append(field.AccessModifier.Origin.Value);
                Space();
                DataTypeToString(field.Type);
                Space();
                builder.Append(field.Name);
                builder.Append(Grammar.Delimiters.CInstruction.Value);
                NewLine();
            }
        }

        private static void DataTypeToString(DataType dataType)
        {
            if (dataType.Settings.Static)
            {
                builder.Append(Grammar.Keywords.Static.Value);
                Space();
            }

            if (dataType.Settings.Constant)
            {
                builder.Append(Grammar.Keywords.Constant.Value);
                Space();
            }
            else if (dataType.Settings.ReadOnly)
            {
                builder.Append(Grammar.Keywords.Readonly.Value);
                Space();
            }
            else if (dataType.Settings.Volatile)
            {
                builder.Append(Grammar.Keywords.Volatile.Value);
                Space();
            }

            builder.Append(dataType.Name);

            if (dataType.Settings.Array)
            {
                builder.Append(Grammar.Delimiters.OIndex);
                builder.Append(Grammar.Delimiters.CIndex);
            }
        }

        private static void NewLine()
        {
            builder.AppendLine();
            Tab();
        }
        private static void Space()
        {
            builder.Append(" ");
        }

        private static void Tab()
        {
            for (int i = 0; i < tabTimes; i++)
            {
                builder.Append('\t');
            }
        }

        private static int tabTimes;

        private static void OBlock()
        {
            NewLine();
            builder.AppendLine(Grammar.Delimiters.OBlock.Value);
            tabTimes++;
            NewLine();
        }

        private static void CBlock()
        {
            tabTimes--;
            NewLine();
            builder.AppendLine(Grammar.Delimiters.CBlock.Value);
            NewLine();
        }
    }
}

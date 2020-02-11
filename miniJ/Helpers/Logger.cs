using miniJ.Elements.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace miniJ.Helpers
{
    class Logger
    {

        private Dictionary<ICompilerNode, StringBuilder> logTable;
        public Logger()
        {
            logTable = new Dictionary<ICompilerNode, StringBuilder>();
        }
        public void Log(string message, ICompilerNode node)
        {
            logTable[node].AppendLine(message);
            Console.WriteLine(message);
        }

        public void CreateLogger(ICompilerNode node)
        {
            logTable.Add(node, new StringBuilder());
        }

        public void AppendToFile(bool byNode = true)
        {
            StreamWriter logFile = new StreamWriter("Log_All.txt");
            logFile.AutoFlush = true;
            logFile.WriteLine(this.ToString());
            logFile.Close();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach(ICompilerNode node in logTable.Keys)
            {
                builder.AppendLine("Log info of " + node.GetType());
                builder.AppendLine(logTable[node].ToString());
                builder.AppendLine();
            }
            return builder.ToString();
        }

    }
}

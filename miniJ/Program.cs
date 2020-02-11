using miniJ.Elements;
using miniJ.Helpers;
using miniJ.Lexical;
using miniJ.Lexical.Elements.Token;
using System;
using System.Collections.Generic;

namespace miniJ
{
     class Program
    {
        private static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;

            Global.Reset();
            Global.LexicalProcess();
           

            Global.Logger.AppendToFile();

            Console.WriteLine("Finished in " + DateTime.Now.Subtract(startTime));

           // System.Diagnostics.Process.Start(Environment.CurrentDirectory);
            Console.ReadKey();
        }
    }
}
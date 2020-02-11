using miniJ.Helpers;
using System;

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

            Console.ReadKey();
        }
    }
}
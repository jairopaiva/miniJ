using miniJ.Elements.Base.CompilationElements;
using miniJ.Lexical;
using miniJ.Parsing;
using System;
using System.IO;

namespace miniJ
{
    static class Program
    {
        private static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;

            CompilationUnit compilationUnit = new CompilationUnit();
            Lexer lexer = new Lexer();
            PreProcessor preProcessor = new PreProcessor();

            string projectFolder = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));

            lexer.Scan(projectFolder+@"\SampleCodes\SyntaxTests.jpl", ref compilationUnit);
            preProcessor.Start(ref compilationUnit);

            DateTime endTime = DateTime.Now;

            if(compilationUnit.Errors.Count > 0)
            {
                Console.WriteLine();
               Console.WriteLine("Detected the following errors during compilation:");
                for(int i = 0; i < compilationUnit.Errors.Count; i++)
                {
                    Console.WriteLine("[" + (i + 1) + "] = " + compilationUnit.Errors[i].ToString());
                }
                Console.WriteLine();
            }

            Console.WriteLine("Finished in " + endTime.Subtract(startTime));

            Console.WriteLine("Recreating the processed program into code...");
            StreamWriter log = new StreamWriter("RecreatedProgram.jpl");
            log.WriteLine(Helpers.Remaker.Recreate(compilationUnit));
            log.Flush();
            log.Close();
            Console.WriteLine("Finished all stages!");
            Console.ReadKey();
        }
    }
}
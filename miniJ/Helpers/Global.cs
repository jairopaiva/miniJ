using miniJ.Elements;
using miniJ.Lexical;
using miniJ.Lexical.Elements.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Helpers
{
   class Global
    {

        public static Logger Logger;
        public static Lexer Lexer;
        public static List<List<LexerToken>> lexerTokenCollection;
        public static Project Project;

        public static void Reset()
        {
            SetUpProject();
            SetUpLexer();
            SetUpLogger();
        }

        private static void SetUpProject()
        {
            Project project = new Project() { Name = "SampleCodes" };
            project.Folders = new List<Folder>()
            {
                Folder.Open(Environment.CurrentDirectory+@"\SampleCodes"),
                 Folder.Open(Environment.CurrentDirectory+@"\SampleCodes\TestPath")
            };
        }

        private static void SetUpLogger()
        {
            Logger = new Logger();
            Logger.CreateLogger(Lexer);
        }

        private static void SetUpLexer()
        {
            lexerTokenCollection = new List<List<LexerToken>>();
            Lexer = new Lexer();
        }

        public static void LexicalProcess()
        {
            foreach (Folder folder in Project.Folders)
            {
                foreach (File file in folder.Files)
                    lexerTokenCollection.Add(Lexer.Scan(folder.Path + file.Name));
            }
        }

    }
}

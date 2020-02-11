using miniJ.Elements;
using miniJ.Lexical;
using miniJ.Lexical.Elements.Token;
using System;
using System.Collections.Generic;

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
            string projectFolder = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
            Project = new Project() { Name = "SampleCodes" };
            Project.Folders = new List<Folder>()
            {
                Folder.Open(projectFolder+@"\SampleCodes"),
                 Folder.Open(projectFolder+@"\SampleCodes\TestPath")
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
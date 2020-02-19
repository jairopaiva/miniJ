﻿using miniJ.Elements;
using miniJ.Grammar;
using miniJ.Lexical;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing;
using System;
using System.Collections.Generic;

namespace miniJ.Helpers
{
    class Global
    {
        public static Logger Logger;
        public static Lexer Lexer;
        public static PreProcessor PreProcessor;
        public static List<Token> lexerTokenCollection;
        public static Dictionary<string, Token> tokenDatabase;   // Dicionário contendo os tokens padrões da linguagem
        public static Project Project;
        public static Namespace GlobalNamespace;

        public static void Reset()
        {
            SetUpGlobalNamespace();
            SetUpTokenDatabase();
            SetUpProject();
            SetUpLexer();
            SetUpLogger();
            SetUpPreProcessor();
        }

        public static void Compile()
        {
            LexicalProcess();
          //  Logger.AppendToFile();
            PreProcessor.Process();
        }

        private static void SetUpGlobalNamespace()
        {
            GlobalNamespace = new Namespace(null, null);
            GlobalNamespace.Name = "global";
        }

        private static void SetUpProject()
        {
            string projectFolder = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
            Project = new Project() { Name = "SampleCodes" };
            Project.Folders = new List<Folder>()
            {
                Folder.Open(projectFolder+@"\SampleCodes")//,
                // Folder.Open(projectFolder+@"\SampleCodes\TestPath")
            };
        }

        private static void SetUpLogger()
        {
            Logger = new Logger();
            Logger.CreateLogger(Lexer);
        }

        private static void SetUpLexer()
        {
            lexerTokenCollection = new List<Token>();
            Lexer = new Lexer();
        }

        private static void SetUpPreProcessor()
        {
            PreProcessor = new PreProcessor();
        }

        public static void LexicalProcess()
        {
            foreach (Folder folder in Project.Folders)
            {
                foreach (File file in folder.Files)
                    lexerTokenCollection.AddRange(Lexer.Scan(folder.Path + file.Name));
            }
            lexerTokenCollection.Add(
                Delimiters.EOF.Copy(lexerTokenCollection[lexerTokenCollection.Count - 1].Location));
        }

        private static void SetUpTokenDatabase()
        {
            tokenDatabase = new Dictionary<string, Token>()
            {
                { AccessModifier.Private.Value, AccessModifier.Private },
                { AccessModifier.Protected.Value, AccessModifier.Protected },
                { AccessModifier.Public.Value, AccessModifier.Public },

                { Comparators.And.Value, Comparators.And },
                { Comparators.Different.Value, Comparators.Different },
                { Comparators.Inequality.Value, Comparators.Inequality },
                { Comparators.Equal.Value, Comparators.Equal },
                { Comparators.GreaterThan.Value, Comparators.GreaterThan },
                { Comparators.LessThan.Value, Comparators.LessThan },
                { Comparators.GreaterThanOrEqual.Value, Comparators.GreaterThanOrEqual },
                { Comparators.LessThanOrEqual.Value, Comparators.LessThanOrEqual },
                { Comparators.Or.Value, Comparators.Or },

                { Delimiters.CBlock.Value, Delimiters.CBlock },
                { Delimiters.CIndex.Value, Delimiters.CIndex },
                { Delimiters.CInstruction.Value, Delimiters.CInstruction },
                { Delimiters.Collon.Value, Delimiters.Collon },
                { Delimiters.Comma.Value, Delimiters.Comma },
                { Delimiters.CParenthesis.Value, Delimiters.CParenthesis },
                { Delimiters.Dot.Value, Delimiters.Dot },
                { Delimiters.OBlock.Value, Delimiters.OBlock },
                { Delimiters.OIndex.Value, Delimiters.OIndex },
                { Delimiters.OParenthesis.Value, Delimiters.OParenthesis },
                { Delimiters.Backslash.Value, Delimiters.Backslash },
                { Delimiters.TwoCollon.Value, Delimiters.TwoCollon },

                { Directives.Define.Value, Directives.Define },
                { Directives.ElseIf.Value, Directives.ElseIf },
                { Directives.EndIf.Value, Directives.EndIf },
                { Directives.If.Value, Directives.If },
                { Directives.IfDefined.Value, Directives.IfDefined },
                { Directives.IfNotDefine.Value, Directives.IfNotDefine },
                { Directives.Include.Value, Directives.Include },
                { Directives.ThrowError.Value, Directives.ThrowError },
                { Directives.UnDef.Value, Directives.UnDef },

                { Keywords.Asm.Value, Keywords.Asm },
                { Keywords.Auto.Value, Keywords.Auto },
                { Keywords.Base.Value, Keywords.Base },
                { Keywords.Break.Value, Keywords.Break },
                { Keywords.Case.Value, Keywords.Case },
                { Keywords.Catch.Value, Keywords.Catch },
                { Keywords.Class.Value, Keywords.Class },
                { Keywords.Constant.Value, Keywords.Constant },
                { Keywords.Continue.Value, Keywords.Continue },
                { Keywords.Default.Value, Keywords.Default },
                { Keywords.Do.Value, Keywords.Do },
                { Keywords.Else.Value, Keywords.Else },
                { Keywords.Enum.Value, Keywords.Enum },
                { Keywords.Extern.Value, Keywords.Extern },
                { Keywords.False.Value, Keywords.False },
                { Keywords.For.Value, Keywords.For },
                { Keywords.If.Value, Keywords.If },
                { Keywords.Import.Value, Keywords.Import },
                { Keywords.Interface.Value, Keywords.Interface},
                { Keywords.Namespace.Value, Keywords.Namespace },
                { Keywords.New.Value, Keywords.New },
                { Keywords.Null.Value, Keywords.Null },
                { Keywords.Readonly.Value, Keywords.Readonly },
                { Keywords.Return.Value, Keywords.Return },
                { Keywords.Signed.Value, Keywords.Signed },
                { Keywords.SizeOf.Value, Keywords.SizeOf },
                { Keywords.Struct.Value, Keywords.Struct },
                { Keywords.Switch.Value, Keywords.Switch},
                { Keywords.This.Value, Keywords.This },
                { Keywords.True.Value, Keywords.True },
                { Keywords.Try.Value, Keywords.Try },
                { Keywords.Unsigned.Value, Keywords.Unsigned },
                { Keywords.Volatile.Value, Keywords.Volatile },
                { Keywords.While.Value, Keywords.While  },

                { Types.Bool.Value, Types.Bool },
                { Types.Byte.Value, Types.Byte },
                { Types.Char.Value, Types.Char },
                { Types.Double.Value, Types.Double },
                { Types.Float.Value, Types.Float },
                { Types.Int.Value, Types.Int },
                { Types.Long.Value, Types.Long },
                { Types.String.Value, Types.String },
                { Types.Void.Value, Types.Void },

                { Operators.Add.Value, Operators.Add },
                { Operators.AddAssign.Value, Operators.AddAssign },
                { Operators.And.Value, Operators.And },
                { Operators.Decrement.Value, Operators.Decrement },
                { Operators.Division.Value, Operators.Division },
                { Operators.Equal.Value, Operators.Equal },
                { Operators.Increment.Value, Operators.Increment },
                { Operators.Or.Value, Operators.Or },
                { Operators.Power.Value, Operators.Power },
                { Operators.Sub.Value, Operators.Sub },
                { Operators.SubAssign.Value, Operators.SubAssign }
            };
        }
    }
}
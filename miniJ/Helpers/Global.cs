using miniJ.Elements;
using miniJ.Elements.Base;
using miniJ.Grammar;
using miniJ.Lexical;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing;
using miniJ.Parsing.Elements;
using System;
using System.Collections.Generic;

namespace miniJ.Helpers
{
    class Global
    {
        public static Namespace GlobalNamespace;
        public static Lexer Lexer;
        public static Logger Logger;
        public static PreProcessor PreProcessor;
        public static Project Project;
        public static Dictionary<string, Token> tokenDatabase;   // Dicionário contendo os tokens padrões da linguagem

        public static CompilationElements compilationElements;
        public static void Compile()
        {
            LexicalProcess();
            List<CodeError> detectedErrors = PreProcessor.Start(compilationElements.LexerResult, GlobalNamespace);
            Logger.Log("", PreProcessor);

            if (detectedErrors.Count != 0)
            {
                Logger.Log("Finished pre-processing with errors:", PreProcessor);
                foreach (CodeError error in detectedErrors)
                {
                    Logger.Log(error.ErrorMessage, PreProcessor);
                }
            }
            else
            {
                Logger.Log("Finished pre-processing without errors!", PreProcessor);
            }

            Logger.Log("", PreProcessor);
        }

        public static void LexicalProcess()
        {
            compilationElements.LexerResult = new LexerResult();
            foreach (Folder folder in Project.Folders)
            {
                foreach (File file in folder.Files)
                {
                    Lexer.Scan(folder.Path + file.Name, compilationElements.LexerResult);
                }
            }

            compilationElements.LexerResult.SetAllTypeIdentifiers();
            Token lastLexerToken = compilationElements.LexerResult.Tokens[compilationElements.LexerResult.Tokens.Count - 1];
            compilationElements.LexerResult.Tokens.Add(Delimiters.EOF.Copy(lastLexerToken.Location));
        }

        public static void Reset()
        {
            SetUpGlobalNamespace();
            SetUpTokenDatabase();
            SetUpProject();
            SetUpLexer();
            SetUpPreProcessor();
            SetUpLogger();
        }

        private static void SetUpGlobalNamespace()
        {
            GlobalNamespace = new Namespace(Keywords.Global, null)
            {
                Name = Keywords.Global.Value
            };
        }

        private static void SetUpLexer()
        {
            compilationElements = new CompilationElements();
            Lexer = new Lexer(tokenDatabase);
        }

        private static void SetUpLogger()
        {
            Logger = new Logger();
            Logger.CreateLogger(Lexer);
            Logger.CreateLogger(PreProcessor);
        }

        private static void SetUpPreProcessor()
        {
            PreProcessor = new PreProcessor();
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

        private static void SetUpTokenDatabase()
        {
            tokenDatabase = new Dictionary<string, Token>()
            {
                { AccessModifier.Private.Value, AccessModifier.Private },
                { AccessModifier.Protected.Value, AccessModifier.Protected },
                { AccessModifier.Public.Value, AccessModifier.Public },

                { Comparators.And.Value, Comparators.And },
                { Comparators.Different.Value, Comparators.Different },
                { Comparators.Equal.Value, Comparators.Equal },
                { Comparators.GreaterThan.Value, Comparators.GreaterThan },
                { Comparators.GreaterThanOrEqual.Value, Comparators.GreaterThanOrEqual },
                { Comparators.Inequality.Value, Comparators.Inequality },
                { Comparators.LessThan.Value, Comparators.LessThan },
                { Comparators.LessThanOrEqual.Value, Comparators.LessThanOrEqual },
                { Comparators.Or.Value, Comparators.Or },

                { Delimiters.Backslash.Value, Delimiters.Backslash },
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
                { Delimiters.TwoCollon.Value, Delimiters.TwoCollon },
                { Delimiters.EOF.Value, Delimiters.EOF },

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
                { Keywords.Interface.Value, Keywords.Interface},
                { Keywords.Namespace.Value, Keywords.Namespace },
                { Keywords.New.Value, Keywords.New },
                { Keywords.Null.Value, Keywords.Null },
                { Keywords.Operator.Value, Keywords.Operator },
                { Keywords.Override.Value, Keywords.Override },
                { Keywords.Readonly.Value, Keywords.Readonly },
                { Keywords.Return.Value, Keywords.Return },
                { Keywords.SizeOf.Value, Keywords.SizeOf },
                { Keywords.Static.Value, Keywords.Static },
                { Keywords.Struct.Value, Keywords.Struct },
                { Keywords.Switch.Value, Keywords.Switch},
                { Keywords.This.Value, Keywords.This },
                { Keywords.True.Value, Keywords.True },
                { Keywords.Try.Value, Keywords.Try },
                { Keywords.Using.Value, Keywords.Using },
                { Keywords.Virtual.Value, Keywords.Virtual },
                { Keywords.Volatile.Value, Keywords.Volatile },
                { Keywords.While.Value, Keywords.While  },

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
                { Operators.SubAssign.Value, Operators.SubAssign },

                { BuiltInType.Bool.Value, BuiltInType.Bool },
                { BuiltInType.Byte.Value, BuiltInType.Byte },
                { BuiltInType.Char.Value, BuiltInType.Char },
                { BuiltInType.Double.Value, BuiltInType.Double },
                { BuiltInType.Float.Value, BuiltInType.Float },
                { BuiltInType.Int.Value, BuiltInType.Int },
                { BuiltInType.UInt.Value, BuiltInType.UInt },
                { BuiltInType.Long.Value, BuiltInType.Long },
                { BuiltInType.ULong.Value, BuiltInType.ULong },
                { BuiltInType.String.Value, BuiltInType.String },
                { BuiltInType.T.Value, BuiltInType.T },
                { BuiltInType.Void.Value, BuiltInType.Void }
            };
        }
    }
}
using miniJ.Elements.Base;
using miniJ.Grammar;
using miniJ.Helpers;
using miniJ.Lexical.Elements;
using miniJ.Lexical.Elements.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Lexical
{
    class Lexer : ICompilerNode
    {
        // Dicionário contendo os tokens padrões da linguagem
        public readonly Dictionary<string, LexerToken> tokenDatabase;

        public Lexer()
        {
            tokenDatabase = new Dictionary<string, LexerToken>()
            {
                { AccessModifier.Private.Value, AccessModifier.Private },
                { AccessModifier.Protected.Value, AccessModifier.Protected },
                { AccessModifier.Public.Value, AccessModifier.Public },

                { Comparators.And.Value, Comparators.And },
                { Comparators.Different.Value, Comparators.Different },
                { Comparators.Equal.Value, Comparators.Equal },
                { Comparators.GreaterThan.Value, Comparators.GreaterThan },
                { Comparators.LessThan.Value, Comparators.LessThan },
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

        // Para cada arquivo processado, estas variáveis são 'resetadas'
        public string CurrentFile { get; set; }

        public string CurrentSourceCode { get; set; }
        public StringReader Reader { get; set; }

        public List<LexerToken> Scan(string filePath)
        {
            CurrentSourceCode = System.IO.File.ReadAllText(filePath);
            Reader = new StringReader(CurrentSourceCode);
            List<LexerToken> Tokens = new List<LexerToken>();
            CurrentFile = filePath;

            while (Reader.Peek() != -1)
            {
                char curChar = (char)Reader.Peek();

                if (LexerUtils.IsNewLine(curChar))
                {
                    Reader.Read();
                    Reader.NewLine();
                }
                else if (char.IsWhiteSpace(curChar))
                {
                    Reader.Read();
                }
                else if (char.IsLetter(curChar))
                {
                    ParseLetter(ref Tokens);
                }
                else if (char.IsDigit(curChar))
                {
                    ParseNumber(ref Tokens);
                }
                else if (curChar == LexerUtils.DIRECTIVE_SIGNAL)
                {
                    ParseDirective(ref Tokens);
                }
                else
                    try
                    {
                        Reader.Read();
                        AddToken(curChar.ToString(), Reader.Column, ref Tokens);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
            }

            return Tokens;
        }

        private void ParseDirective(ref List<LexerToken> tokens)
        {
            int start = Reader.Column;
            StringBuilder builder = new StringBuilder();
            builder.Append((char)Reader.Peek()); // Escreve o símbolo '#' para que a função AddToken saiba de que se trata de um Token do tipo Directive

            Reader.Read();

            char curChar = (char)Reader.Peek();

            while (char.IsLetter(curChar))
            {
                builder.Append(curChar);
                Reader.Read();
                if (Reader.Peek() != -1)
                { curChar = (char)Reader.Peek(); }
                else
                { break; }
            }

            AddToken(builder.ToString(), start, ref tokens);
        }

        private void ParseNumber(ref List<LexerToken> tokens)
        {
            int start = Reader.Column;
            char curChar = (char)Reader.Peek();
            Reader.Read();

            StringBuilder builder = new StringBuilder();
            bool hexNumber = curChar == LexerUtils.HEX_SIGNAL[0] && (char)Reader.Peek() == LexerUtils.HEX_SIGNAL[1];

            if (hexNumber)
            {
                while (LexerUtils.HexChar(curChar))
                {
                    builder.Append(curChar);
                    Reader.Read();
                    if (Reader.Peek() != -1)
                    { curChar = (char)Reader.Peek(); }
                    else
                    {
                        break;
                    }
                }
                string converted = Convert.ToUInt64(builder.ToString(), 16).ToString();
                builder.Clear();
                builder.Append(converted);
            }
            else
            {
                bool dotDetected = false;
                while (char.IsDigit(curChar) || curChar == LexerUtils.DOT)
                {
                    builder.Append(curChar);
                    Reader.Read();
                    if (Reader.Peek() != -1)
                    { curChar = (char)Reader.Peek(); }
                    else
                    {
                        break;
                    }

                    if (curChar == LexerUtils.DOT && dotDetected)
                        throw new Exception();
                    else
                        dotDetected = true;
                }
            }

            AddToken(builder.ToString(), start, ref tokens, TokenType.Number);
        }

        private void ParseLetter(ref List<LexerToken> tokens)
        {
            char curChar = (char)Reader.Peek();
            StringBuilder builder = new StringBuilder();

            int start = Reader.Column;

            while (char.IsLetter(curChar) || curChar == LexerUtils.UNDERLINE)
            {
                builder.Append(curChar);
                Reader.Read();
                if (Reader.Peek() != -1)
                { curChar = (char)Reader.Peek(); }
                else
                { break; }
            }

            AddToken(builder.ToString(), start, ref tokens);
        }

        private void AddToken(string Value, int startPos, ref List<LexerToken> tokens, TokenType tokenType = TokenType.Identifier)
        {
            TokenLocation location = new TokenLocation() { Column = startPos, Line = Reader.Line, File = CurrentFile };
            LexerToken token = new LexerToken(Value) { Location = location, Type = tokenType };

            if (tokenDatabase.ContainsKey(Value))
            {
                token.Type = tokenDatabase[Value].Type;
            }

            Global.Logger.Log(token.ToString(), this);
            tokens.Add(token);
        }
    }
}
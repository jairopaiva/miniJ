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
        private readonly Dictionary<string, LexerToken> tokenDatabase;

        public Lexer()
        {
            tokenDatabase = Global.tokenDatabase;
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
                    Reader.NewLine();
                    Reader.Read();
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
                else if (curChar == Directives.Define.Value[0])
                {
                    ParseDirective(ref Tokens);
                }
                else if (LexerUtils.IsOperatorOrComparator(curChar.ToString()))
                {
                    ParseSymbolOrOperator(ref Tokens);
                }
                else if (curChar == Delimiters.CharAssigment.Value[0])
                {
                    ParseCharAssigment(ref Tokens);
                }
                else if (curChar == Delimiters.StringAssigment.Value[0])
                {
                    ParseStringAssigment(ref Tokens);
                }
                else
                    try
                    {
                        AddToken(curChar.ToString(), Reader.Column, ref Tokens);
                        Reader.Read();
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
            char curChar = (char)Reader.Read();

            int start = Reader.Column;
            StringBuilder builder = new StringBuilder();
            builder.Append(curChar); // Escreve o símbolo '#' para que a função AddToken saiba de que se trata de um Token do tipo Directive

            curChar = (char)Reader.Peek();

            while (char.IsLetter(curChar))
            {
                builder.Append(curChar);
                Reader.Read();

                if (Reader.Peek() != -1)
                {
                    curChar = (char)Reader.Peek();
                }
                else
                {
                    break;
                }
            }

            AddToken(builder.ToString(), start, ref tokens);
        }

        private void ParseNumber(ref List<LexerToken> tokens)
        {
            int start = Reader.Column;
            char curChar = (char)Reader.Peek(); ;

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
                    {
                        curChar = (char)Reader.Peek();
                    }
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
                while (char.IsDigit(curChar) || curChar == Delimiters.Dot.Value[0])
                {
                    builder.Append(curChar);
                    Reader.Read();

                    if (Reader.Peek() != -1)
                    {
                        curChar = (char)Reader.Peek();
                    }
                    else
                    {
                        break;
                    }

                    if (curChar == Delimiters.Dot.Value[0] && dotDetected)
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
                {
                    curChar = (char)Reader.Peek();
                }
                else
                {
                    break;
                }
            }

            AddToken(builder.ToString(), start, ref tokens);
        }

        private void ParseSymbolOrOperator(ref List<LexerToken> tokens)
        {
            string curChar = ((char)Reader.Peek()).ToString();
            StringBuilder builder = new StringBuilder();

            int start = Reader.Column;

            while (LexerUtils.IsOperatorOrComparator(curChar))
            {
                builder.Append(curChar);
                Reader.Read();

                if (Reader.Peek() != -1)
                {
                    curChar = ((char)Reader.Peek()).ToString();
                }
                else
                {
                    break;
                }
            }

            AddToken(builder.ToString(), start, ref tokens);
        }

        private void ParseStringAssigment(ref List<LexerToken> tokens)
        {
            int start = Reader.Column;
            Reader.Read();
            char curChar = (char)Reader.Peek();
            StringBuilder builder = new StringBuilder();

            while (curChar != Delimiters.StringAssigment.Value[0])
            {
                builder.Append(curChar);
                Reader.Read();

                if (Reader.Peek() != -1)
                {
                    curChar = (char)Reader.Peek();
                }
                else
                {
                    break;
                }
            }

            Reader.Read();

            AddToken(builder.ToString(), start, ref tokens, TokenType.String);
        }

        private void ParseCharAssigment(ref List<LexerToken> tokens)
        {
            int start = Reader.Column;
            Reader.Read();
            char curChar = (char)Reader.Peek();
            StringBuilder builder = new StringBuilder();

            while (curChar != Delimiters.CharAssigment.Value[0])
            {
                builder.Append(curChar);
                Reader.Read();

                if (Reader.Peek() != -1)
                {
                    curChar = (char)Reader.Peek();
                }
                else
                {
                    break;
                }
            }

            Reader.Read();

            AddToken(builder.ToString(), start, ref tokens, TokenType.Char);
        }

        private void AddToken(string Value, int startPos, ref List<LexerToken> tokens, TokenType tokenType = TokenType.None)
        {
            TokenLocation location = new TokenLocation() { Column = startPos, Line = Reader.Line, File = CurrentFile };
            LexerToken token = new LexerToken(Value) { Location = location, Type = tokenType }; ;

            if (tokenDatabase.ContainsKey(Value))
            {
                token.Type = tokenDatabase[Value].Type;
            }
            else
            {
                if (token.Type == TokenType.None)
                {
                    token.Type = TokenType.Identifier;
                }
            }

            Global.Logger.Log(token.ToString(), this);
            tokens.Add(token);
        }
    }
}
﻿using miniJ.Elements.Base;
using miniJ.Grammar;
using miniJ.Helpers;
using miniJ.Lexical.Elements;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing;
using miniJ.Parsing.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Lexical
{
    /// <summary> https://pt.wikipedia.org/wiki/An%C3%A1lise_l%C3%A9xica
    /// Análise léxica é o processo de analisar a entrada de linhas de caracteres (tal como o código-fonte de um programa
    /// de computador) e produzir uma sequência de símbolos chamado "símbolos léxicos" (lexical tokens), ou somente
    /// "símbolos" (tokens), que podem ser manipulados mais facilmente por um parser (leitor de saída).
    /// </summary>
    class Lexer : ICompilerNode
    {
        private readonly Dictionary<string, Token> tokenDatabase;

        public Lexer()
        {
            tokenDatabase = Global.tokenDatabase;
        }

        // Para cada arquivo processado, estas variáveis são 'resetadas'
        public string CurrentFile { get; set; }

        public string CurrentSourceCode { get; set; }
        public StringReader Reader { get; set; }
        private bool _nextTokenCISEIdentifier;
        private CISE.SpecificTypeOfCISE _nextTokenCISEType;

        public List<Token> Scan(string filePath)
        {
            CurrentSourceCode = System.IO.File.ReadAllText(filePath);
            Reader = new StringReader(CurrentSourceCode);
            List<Token> Tokens = new List<Token>();
            CurrentFile = filePath;
            _nextTokenCISEIdentifier = false;

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

        private void AddToken(string Value, int startPos, ref List<Token> tokens, TokenType specificTokenType = TokenType.NotDef_None)
        {
            NodeLocation location = new NodeLocation() { Column = startPos, Line = Reader.Line, File = CurrentFile };
            Token token = new Token(Value) { Location = location, TokenType = specificTokenType }; ;

            if (tokenDatabase.ContainsKey(Value))
            {
                token.TokenType = tokenDatabase[Value].TokenType;

                if (ParserUtils.IsCISE(token))
                {
                    _nextTokenCISEIdentifier = true;
                    _nextTokenCISEType = ParserUtils.GetCISEType(token);
                }
            }
            else
            {
                if (token.TokenType == TokenType.NotDef_None)
                {
                    if (!_nextTokenCISEIdentifier)
                        token.TokenType = TokenType.NotDef_Identifier;
                    else
                    {
                        token.TokenType = TokenType.NotDef_TypeIdentifier;
                        CISE cise = new CISE(token, _nextTokenCISEType, token);
                        Helpers.Global.cisesDetectedInLexer.Add(cise);
                        _nextTokenCISEIdentifier = false;
                    }
                }
            }

            //  Global.Logger.Log(token.ToString(), this);
            tokens.Add(token);
        }

        private void ParseCharAssigment(ref List<Token> tokens)
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

            AddToken(builder.ToString(), start, ref tokens, TokenType.NotDef_Char);
        }

        private void ParseDirective(ref List<Token> tokens)
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

        private void ParseLetter(ref List<Token> tokens)
        {
            char curChar = (char)Reader.Peek();
            StringBuilder builder = new StringBuilder();

            int start = Reader.Column;

            while (char.IsLetterOrDigit(curChar) || curChar == LexerUtils.UNDERLINE)
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

        private void ParseNumber(ref List<Token> tokens)
        {
            int start = Reader.Column;
            char curChar = (char)Reader.Peek();

            StringBuilder builder = new StringBuilder();

            Reader.Reset();
            Reader.Next();

            bool hexNumber = curChar == LexerUtils.HEX_SIGNAL[0] && (char)Reader.PeekTemp() == LexerUtils.HEX_SIGNAL[1];

            if (hexNumber)
            {
                Reader.Read();
                Reader.Read();
                curChar = (char)Reader.Peek();
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

                    if (curChar == Delimiters.Dot.Value[0])
                        if (dotDetected)
                        {
                            throw new Exception();
                        }
                        else
                            dotDetected = true;
                }
            }

            AddToken(builder.ToString(), start, ref tokens, TokenType.NotDef_Number);
        }

        private void ParseStringAssigment(ref List<Token> tokens)
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

            AddToken(builder.ToString(), start, ref tokens, TokenType.NotDef_String);
        }

        private void ParseSymbolOrOperator(ref List<Token> tokens)
        {
            string curChar = ((char)Reader.Peek()).ToString();
            Reader.Reset(); Reader.Next();
            if (curChar == Operators.Division.Value &&
                ((char)Reader.PeekTemp()).ToString() == Operators.Division.Value) // Se trata de um comentário
            {
                while (!LexerUtils.IsNewLine((char)Reader.Peek()))
                {
                    if (Reader.Read() == -1)
                        break;
                }
                return;
            }

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
    }
}
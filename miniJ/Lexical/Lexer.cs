using miniJ.Elements.Base;
using miniJ.Grammar;
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
        private readonly Dictionary<string, Token> _tokenDatabase;
        public Lexer(Dictionary<string, Token> tokenDatabase)
        {
            _tokenDatabase = tokenDatabase;
        }

        // Para cada arquivo processado, estas variáveis são 'resetadas'
        private CISE.SpecificTypeOfCISE _nextTokenCISEType;
        private bool _nextTokenCISEIdentifier;
        private string _currentSourceCode;
        private string _currentFile;
        private StringReader _reader;

        public void Scan(string filePath, LexerResult lexerResult)
        {
            _currentSourceCode = System.IO.File.ReadAllText(filePath);
            _reader = new StringReader(_currentSourceCode);
            _nextTokenCISEIdentifier = false;
            _currentFile = filePath;

            while (_reader.Peek() != -1)
            {
                char curChar = (char)_reader.Peek();

                if (LexerUtils.IsNewLine(curChar))
                {
                    _reader.NewLine();
                    _reader.Read();
                }
                else if (char.IsWhiteSpace(curChar) || curChar == LexerUtils.TAB_CHAR)
                {
                    if (curChar == LexerUtils.TAB_CHAR)
                    {
                        _reader.Read();
                        _reader.Column += 3;
                    }
                    else
                    {
                        _reader.Read();
                    }
                }
                else if (char.IsLetter(curChar) || curChar == LexerUtils.UNDERLINE)
                {
                    ParseLetter(ref lexerResult);
                }
                else if (char.IsDigit(curChar))
                {
                    ParseNumber(ref lexerResult);
                }
                else if (curChar == Directives.Define.Value[0])
                {
                    ParseDirective(ref lexerResult);
                }
                else if (LexerUtils.IsOperatorOrComparator(curChar.ToString()))
                {
                    ParseSymbolOrOperator(ref lexerResult);
                }
                else if (curChar == Delimiters.CharAssigment.Value[0])
                {
                    ParseCharAssigment(ref lexerResult);
                }
                else if (curChar == Delimiters.StringAssigment.Value[0])
                {
                    ParseStringAssigment(ref lexerResult);
                }
                else
                    try
                    {
                        AddToken(curChar.ToString(), _reader.Column, ref lexerResult);
                        _reader.Read();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
            }
        }

        private void AddToken(string Value, int startPos, ref LexerResult lexerResult, TokenType specificTokenType = TokenType.NotDef_None)
        {
            NodeLocation location = new NodeLocation() { Column = startPos, Line = _reader.Line, File = _currentFile };
            Token token = new Token(Value) { Location = location, TokenType = specificTokenType }; ;

            if (_tokenDatabase.ContainsKey(Value))
            {
                token.TokenType = _tokenDatabase[Value].TokenType;

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
                    {
                        if (!ParserUtils.ValidIdentifier(token.Value))
                        {
                            throw new Exception(token.ToString());
                        }

                        token.TokenType = TokenType.NotDef_Identifier;
                    }
                    else
                    {
                        token.TokenType = TokenType.NotDef_Identifier;
                        CISE cise = new CISE(token, _nextTokenCISEType, token);
                        lexerResult.CISES.Add(cise);
                        _nextTokenCISEIdentifier = false;
                    }
                }
            }

            //  Global.Logger.Log(token.ToString(), this);
            lexerResult.Tokens.Add(token);
        }

        private void ParseCharAssigment(ref LexerResult lexerResult)
        {
            int start = _reader.Column;
            _reader.Read();
            char curChar = (char)_reader.Peek();
            StringBuilder builder = new StringBuilder();

            while (curChar != Delimiters.CharAssigment.Value[0])
            {
                builder.Append(curChar);
                _reader.Read();

                if (_reader.Peek() != -1)
                {
                    curChar = (char)_reader.Peek();
                }
                else
                {
                    break;
                }
            }

            _reader.Read();

            AddToken(builder.ToString(), start, ref lexerResult, TokenType.NotDef_Char);
        }

        private void ParseDirective(ref LexerResult lexerResult)
        {
            char curChar = (char)_reader.Read();

            int start = _reader.Column;
            StringBuilder builder = new StringBuilder();
            builder.Append(curChar); // Escreve o símbolo '#' para que a função AddToken saiba de que se trata de um Token do tipo Directive

            curChar = (char)_reader.Peek();

            while (char.IsLetter(curChar))
            {
                builder.Append(curChar);
                _reader.Read();

                if (_reader.Peek() != -1)
                {
                    curChar = (char)_reader.Peek();
                }
                else
                {
                    break;
                }
            }

            AddToken(builder.ToString(), start, ref lexerResult);
        }

        private void ParseLetter(ref LexerResult lexerResult)
        {
            char curChar = (char)_reader.Peek();
            StringBuilder builder = new StringBuilder();

            int start = _reader.Column;

            while (char.IsLetterOrDigit(curChar) || curChar == LexerUtils.UNDERLINE)
            {
                builder.Append(curChar);
                _reader.Read();

                if (_reader.Peek() != -1)
                {
                    curChar = (char)_reader.Peek();
                }
                else
                {
                    break;
                }
            }

            AddToken(builder.ToString(), start, ref lexerResult);
        }

        private void ParseNumber(ref LexerResult lexerResult)
        {
            int start = _reader.Column;
            char curChar = (char)_reader.Peek();

            StringBuilder builder = new StringBuilder();

            _reader.Reset();
            _reader.Next();

            bool hexNumber = curChar == LexerUtils.HEX_SIGNAL[0] && (char)_reader.PeekTemp() == LexerUtils.HEX_SIGNAL[1];

            if (hexNumber)
            {
                _reader.Read();
                _reader.Read();
                curChar = (char)_reader.Peek();
                while (LexerUtils.HexChar(curChar))
                {
                    builder.Append(curChar);
                    _reader.Read();

                    if (_reader.Peek() != -1)
                    {
                        curChar = (char)_reader.Peek();
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
                    _reader.Read();

                    if (_reader.Peek() != -1)
                    {
                        curChar = (char)_reader.Peek();
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

            AddToken(builder.ToString(), start, ref lexerResult, TokenType.NotDef_Number);
        }

        private void ParseStringAssigment(ref LexerResult lexerResult)
        {
            int start = _reader.Column;
            _reader.Read();
            char curChar = (char)_reader.Peek();
            StringBuilder builder = new StringBuilder();

            while (curChar != Delimiters.StringAssigment.Value[0])
            {
                builder.Append(curChar);
                _reader.Read();

                if (_reader.Peek() != -1)
                {
                    curChar = (char)_reader.Peek();
                }
                else
                {
                    break;
                }
            }

            _reader.Read();

            AddToken(builder.ToString(), start, ref lexerResult, TokenType.NotDef_String);
        }

        private void ParseSymbolOrOperator(ref LexerResult lexerResult)
        {
            string curChar = ((char)_reader.Peek()).ToString();
            _reader.Reset(); _reader.Next();
            if (curChar == Operators.Division.Value &&
                ((char)_reader.PeekTemp()).ToString() == Operators.Division.Value) // Se trata de um comentário
            {
                while (!LexerUtils.IsNewLine((char)_reader.Peek()))
                {
                    if (_reader.Read() == -1)
                        break;
                }
                return;
            }

            StringBuilder builder = new StringBuilder();

            int start = _reader.Column;

            while (LexerUtils.IsOperatorOrComparator(curChar))
            {
                builder.Append(curChar);
                _reader.Read();

                if (_reader.Peek() != -1)
                {
                    curChar = ((char)_reader.Peek()).ToString();
                }
                else
                {
                    break;
                }
            }

            AddToken(builder.ToString(), start, ref lexerResult);
        }

    }
}
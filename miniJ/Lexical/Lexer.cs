using miniJ.Elements.Base;
using miniJ.Elements.Base.CompilationElements;
using miniJ.Elements.Base.Error;
using miniJ.Lexical.Elements;
using miniJ.Lexical.Elements.Token;
using miniJ.Parsing;
using System;
using System.Text;

namespace miniJ.Lexical
{

    /// <summary> https://pt.wikipedia.org/wiki/An%C3%A1lise_l%C3%A9xica
    /// Análise léxica é o processo de analisar a entrada de linhas de caracteres (tal como o código-fonte de um programa
    /// de computador) e produzir uma sequência de símbolos chamado "símbolos léxicos" (lexical tokens), ou somente
    /// "símbolos" (tokens), que podem ser manipulados mais facilmente por um parser (leitor de saída).
    /// </summary>
    public class Lexer : ICompilerNode
    {
        public Lexer()
        {
        }

        private bool _nextTokenCISEIdentifier;
        private Token _nextTokenCISEType;
        private StringReader _reader;

        public void Scan(string filePath, ref CompilationUnit compilationUnit)
        {
            _reader = new StringReader(filePath);
            _nextTokenCISEIdentifier = false;

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
                    ParseLetter(ref compilationUnit);
                }
                else if (char.IsDigit(curChar))
                {
                    ParseNumber(ref compilationUnit);
                }
                else if (curChar == Grammar.Directives.Define.Value[0])
                {
                    ParseDirective(ref compilationUnit);
                }
                else if (LexerUtils.IsOperatorOrComparator(curChar.ToString()))
                {
                    ParseSymbolOrOperator(ref compilationUnit);
                }
                else if (curChar == Grammar.Delimiters.CharAssigment.Value[0])
                {
                    ParseCharAssigment(ref compilationUnit);
                }
                else if (curChar == Grammar.Delimiters.StringAssigment.Value[0])
                {
                    ParseStringAssigment(ref compilationUnit);
                }
                else
                    try
                    {
                        AddToken(curChar.ToString(), _reader.Column, ref compilationUnit);
                        _reader.Read();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
            }
        }

        private void AddToken(string Value, int startPos, ref CompilationUnit compilationUnit, TokenType specificTokenType = TokenType.NotDef_None)
        {
            NodeLocation location = new NodeLocation() { Column = startPos, Line = _reader.Line, File =  _reader.Filename};
            Token token = new Token(Value, specificTokenType) { Location = location }; ;

            if (Grammar.tokenDatabase.ContainsKey(Value))
            {
                token.TokenType = Grammar.tokenDatabase[Value].TokenType;

                if (ParserUtils.IsCISE(token))
                {
                    _nextTokenCISEIdentifier = true;
                    _nextTokenCISEType = token;
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
                        compilationUnit.LexerResult.CISES.Add(new LexerResult.CISEDetectedInLexer(_nextTokenCISEType, token));
                        _nextTokenCISEIdentifier = false;
                    }
                }
            }

            compilationUnit.LexerResult.Tokens.Add(token);
        }

        private void ParseCharAssigment(ref CompilationUnit compilationUnit)
        {
            int start = _reader.Column;
            _reader.Read();
            char curChar = (char)_reader.Peek();
            StringBuilder builder = new StringBuilder();

            while (curChar != Grammar.Delimiters.CharAssigment.Value[0])
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

            AddToken(builder.ToString(), start, ref compilationUnit, TokenType.NotDef_Char);
        }

        private void ParseDirective(ref CompilationUnit compilationUnit)
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

            AddToken(builder.ToString(), start, ref compilationUnit);
        }

        private void ParseLetter(ref CompilationUnit compilationUnit)
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

            AddToken(builder.ToString(), start, ref compilationUnit);
        }

        private void ParseNumber(ref CompilationUnit compilationUnit)
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
                while (char.IsDigit(curChar) || curChar == Grammar.Delimiters.Dot.Value[0])
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

                    if (curChar == Grammar.Delimiters.Dot.Value[0])
                        if (dotDetected)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            dotDetected = true;
                        }
                }
            }

            AddToken(builder.ToString(), start, ref compilationUnit, TokenType.NotDef_Number);
        }

        private void ParseStringAssigment(ref CompilationUnit compilationUnit)
        {
            int start = _reader.Column;
            _reader.Read();
            char curChar = (char)_reader.Peek();
            StringBuilder builder = new StringBuilder();

            while (curChar != Grammar.Delimiters.StringAssigment.Value[0])
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

            AddToken(builder.ToString(), start, ref compilationUnit, TokenType.NotDef_String);
        }

        private void ParseSymbolOrOperator(ref CompilationUnit compilationUnit)
        {
            NodeLocation location = new NodeLocation() { Column = _reader.Column, Line = _reader.Line, File = _reader.Filename };
            string curChar = ((char)_reader.Peek()).ToString();
            _reader.Reset(); _reader.Next();
            if (curChar == Grammar.Operators.Division.Value &&
                ((char)_reader.PeekTemp()).ToString() == Grammar.Operators.Division.Value) // Se trata de um comentário
            {
                while (!LexerUtils.IsNewLine((char)_reader.Peek()))
                {
                    if (_reader.Read() == -1)
                    {
                        break;
                    }
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

            if (Grammar.tokenDatabase.ContainsKey(builder.ToString()))
            {
                AddToken(builder.ToString(), start, ref compilationUnit);
            }
            else
            {

                LexerError error = new LexerError(this, builder.ToString(), location.ToString(), "Unrecognized symbol/operator!");
                compilationUnit.Errors.Add(error);
            }

        }

    }
}
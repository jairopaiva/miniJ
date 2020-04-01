using miniJ.Lexical.Elements.Token;
using miniJ.Parsing.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace miniJ.Elements.Base
{
    class CompilationElements
    {
        public LexerResult LexerResult;

        public CompilationElements()
        {
            LexerResult = new LexerResult();
        }
    }

    class LexerResult
    {
        public List<Token> Tokens;
        public List<CISE> CISES;
        public LexerResult()
        {
            Tokens = new List<Token>();
            CISES = new List<CISE>();
        }

        /// <summary>
        /// Invocado somente depois de todo o código ter passado pela análise léxica. Faz
        /// com que todos os tokens que se refiram a um CISE sejam assinalados com o tipo correto.
        /// </summary>
        public void SetAllTypeIdentifiers()
        {
            HashSet<string> cisesNames = new HashSet<string>();
            for (int i = 0; i < CISES.Count; i++)
            {
                cisesNames.Add(CISES[i].Name);
            }

            for (int i = 0; i < Tokens.Count; i++)
            {
                if (Tokens[i].TokenType == TokenType.NotDef_Identifier
                    && cisesNames.Contains(Tokens[i].Value))
                {
                    Tokens[i].TokenType = TokenType.NotDef_TypeIdentifier;
                }
            }
        }
    }
}

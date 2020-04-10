using miniJ.Lexical.Elements.Token;
using System.Collections.Generic;

namespace miniJ.Elements.Base.CompilationElements
{
    public class LexerResult
    {
        public List<Token> Tokens;
        public List<CISEDetectedInLexer> CISES;
        public LexerResult()
        {
            Tokens = new List<Token>();
            CISES = new List<CISEDetectedInLexer>();
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
                cisesNames.Add(CISES[i].Name.Value);
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

        public struct CISEDetectedInLexer
        {
            public Token Origin;
            public Token Name;
            public CISEDetectedInLexer(Token origin, Token name)
            {
                Origin = origin;
                Name = name;
            }
        }
    }
}

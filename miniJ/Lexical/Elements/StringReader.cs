﻿namespace miniJ.Lexical.Elements
{
    class StringReader
    {
        public readonly string Filename;
        private readonly string Source;
        private int tempPos = 0;

        public StringReader(string fileName)
        {
            Source = System.IO.File.ReadAllText(fileName);
            Filename = fileName;
            Line = 1;
            Column = 1;
        }

        public int Column;
        public int Line;
        public int Position;

        public int this[int Index]
        {
            get
            {
                return (int)Source[Index];
            }
        }

        public void NewLine()
        {
            Line++;
            Column = 0;
        }

        /// <summary>
        /// Retorna o caractere avançando uma posição.
        /// </summary>
        public int Next()
        {
            return (int)Source[tempPos++];
        }

        public int Peek()
        {
            if (Position >= Source.Length)
                return -1;
            return (int)Source[Position];
        }

        /// <summary>
        /// Retorna o caractere a posição temporária
        /// </summary>
        public int PeekTemp()
        {
            return (int)Source[tempPos];
        }

        /// <summary>
        /// Retorna o caractere voltando uma posição.
        /// </summary>
        public int Previous()
        {
            return (int)Source[tempPos--];
        }

        public int Read()
        {
            if (Position >= Source.Length)
                return -1;
            Column++;
            return (int)Source[Position++];
        }

        /// <summary>
        /// Iguala a posição temporária com a atual.
        /// </summary>
        public void Reset()
        {
            tempPos = Position;
        }
    }
}
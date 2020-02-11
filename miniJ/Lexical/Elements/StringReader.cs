namespace miniJ.Lexical.Elements
{
    class StringReader
    {
        private readonly string Source;

        public int Position { get; set; }

        private int tempPos = 0;

        public int Line { get; set; }
        public int Column { get; set; }

        public StringReader(string s)
        {
            Source = s;
            Line = 1;
            Column = 1;
        }

        /// <summary>
        /// O próximo caractere é retornado sem alterar a posição atual, caso seja o último da
        /// lista, o -1 é retornado.
        /// </summary>
        public int Peek()
        {
            if (Position == Source.Length)
                return -1;
            return (int)Source[Position];
        }

        public void NewLine()
        {
            Line++;
            Column = 0;
        }

        /// <summary>
        /// O próximo caractere é retornado caso seja o último da, lista, o -1 é retornado.
        /// </summary>
        public int Read()
        {
            if (Position == Source.Length)
                return -1;
            Column++;
            return (int)Source[Position++];
        }

        /// <summary>
        /// Retorna o caractere armazenado no index passado
        /// </summary>
        public int this[int Index]
        {
            get
            {
                return (int)Source[Index];
            }
        }

        /// <summary>
        /// Iguala a posição temporária com a atual.
        /// </summary>
        public void Reset()
        {
            tempPos = Position;
        }

        /// <summary>
        /// Retorna o caractere avançando uma posição.
        /// </summary>
        public int Next()
        {
            return (int)Source[tempPos++];
        }

        /// <summary>
        /// Retorna o caractere voltando uma posição.
        /// </summary>
        public int Previous()
        {
            return (int)Source[tempPos--];
        }
    }
}
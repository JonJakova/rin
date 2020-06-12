namespace rin
{
    class Lexer
    {
        public string Source { get; private set; }  
        public char CurChar { get; private set; }
        public int CurPos { get; private set; }

        public Lexer(string input)
        {
            Source = input;
            CurChar = '\0';
            CurPos = -1;
            NextChar();
        }

        public void NextChar()
        {
            CurPos += 1;
            if(CurPos >= Source.Length){
                CurChar = '\0';
            }
            else
            {
                CurChar = Source[CurPos];
            }
        }

        public char Peek()
        {
            if (CurPos + 1 >= Source.Length)
            {
                return '\0';
            }
            return Source[CurPos+1];
        }

        public void Abort(string message)
        {

        }

        public void SkipWhitespace()
        {

        }

        public void SkipComment()
        {

        }

        public void GetToken()
        {

        }
    }
}

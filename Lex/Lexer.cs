using System;
using System.Data;

namespace rin
{
    partial class Lexer
    {
        public string Source { get; private set; }  
        public char CurChar { get; private set; }
        public int CurPos { get; private set; }

        private Token _token;
        private char _lastChar;
        private int _startPos;
        private string _tokText;
        private int _substringLength;
        private TokenType _keyword;

        public Lexer(string input)
        {
            Source = input + '\n'; //Source code to lex as a string. Append a newline to simplify lexing/parsing the last token/statement.
            CurChar = '\0'; //Current character in the string.
            CurPos = -1; //Current position in the string.
            NextChar();
        }

        //Process the next character.
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

        //Return the lookahead character.
        public char Peek()
        {
            if (CurPos + 1 >= Source.Length)
            {
                return '\0';
            }
            return Source[CurPos+1];
        }

        //Invalid token found, print error message and exit.
        public void Abort(string message)
        {
            throw new InvalidExpressionException("Lexing error. " + message);
            //Environment.Exit(-1);
        }

        //Skip whitespace except newlines, which we will use to indicate the end of a statement.
        public void SkipWhitespace()
        {
            while (CurChar == ' ' || CurChar == '\t' || CurChar == '\r')
            {
                NextChar();
            }
        }

        //Skip comments in the code.
        public void SkipComment()
        {
            if (CurChar == '#')
            {
                while (CurChar != '\n')
                {
                    NextChar();
                }
            }
        }
    }
}

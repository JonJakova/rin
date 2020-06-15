using System;
using System.Data;

namespace rin
{
    class Lexer
    {
        public string Source { get; private set; }  
        public char CurChar { get; private set; }
        public int CurPos { get; private set; }

        private Token _token;

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

        }

        //Return the next token.
        public Token GetToken()
        {
            SkipWhitespace();
            _token = null;

            //Check the first character of this token to see if we can decide what it is.
            //If it is a multiple character operator (e.g., !=), number, identifier, or keyword then we will process the rest.
            if (CurChar == '+')
            {
                _token = new Token(CurChar, TokenType.PLUS);
            }
            else if (CurChar == '-')
            {
                _token = new Token(CurChar, TokenType.MINUS);
            }
            else if (CurChar == '*')
            {
                _token = new Token(CurChar, TokenType.ASTERISK);
            }
            else if (CurChar == '/')
            {
                _token = new Token(CurChar, TokenType.SLASH);
            }
            else if (CurChar == '\n')
            {
                _token = new Token(CurChar, TokenType.NEWLINE);
            }

            else if (CurChar == '\0')
            {
                _token = new Token(CurChar, TokenType.EOF);
            }
            else
            {
                //Unknown Token
                Abort("Unknown token: " + CurChar);
            }

            NextChar();
            return _token;
        }
    }
}

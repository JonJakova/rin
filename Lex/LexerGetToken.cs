using System;
using System.Data;

namespace rin
{
    partial class Lexer
    {
        //Return the next token.
        public Token GetToken()
        {
            SkipWhitespace();
            SkipComment();
            _token = null;

            //Check the first character of this token to see if we can decide what it is.
            //If it is a multiple character operator (e.g., !=), number, identifier, or keyword then we will process the rest.
            if (CurChar == '+')
            {
                _token = new Token(Char.ToString(CurChar), TokenType.PLUS);
            }
            else if (CurChar == '-')
            {
                _token = new Token(Char.ToString(CurChar), TokenType.MINUS);
            }
            else if (CurChar == '*')
            {
                _token = new Token(Char.ToString(CurChar), TokenType.ASTERISK);
            }
            else if (CurChar == '/')
            {
                _token = new Token(Char.ToString(CurChar), TokenType.SLASH);
            }
            else if (CurChar == '=')
            {
                //Check whether this token is = or ==
                if (Peek() == '=')
                {
                    _lastChar = CurChar;
                    NextChar();
                    _token = new Token(Char.ToString(_lastChar) + Char.ToString(CurChar), TokenType.EQEQ);
                }
                else
                {
                    _token = new Token(Char.ToString(CurChar), TokenType.EQ);
                }
            }
            else if (CurChar == '>')
            {
                //Check whether this is token is > or >=
                if (Peek() == '=')
                {
                    _lastChar = CurChar;
                    NextChar();
                    _token = new Token(Char.ToString(_lastChar) + Char.ToString(CurChar), TokenType.GTEQ);
                }
                else
                {
                    _token = new Token(Char.ToString(CurChar), TokenType.GT);
                }
            }
            else if (CurChar == '<')
            {
                //Check whether this is token is < or <=
                if (Peek() == '=')
                {
                    _lastChar = CurChar;
                    NextChar();
                    _token = new Token(Char.ToString(_lastChar) + Char.ToString(CurChar), TokenType.LTEQ);
                }
                else
                {
                    _token = new Token(Char.ToString(CurChar), TokenType.LT);
                }
            }
            else if (CurChar == '!')
            {
                // != is different because it is valid only when followed by = 
                if (Peek() == '=')
                {
                    _lastChar = CurChar;
                    NextChar();
                    _token = new Token(Char.ToString(_lastChar) +Char.ToString(CurChar), TokenType.NOTEQ);
                }
                else
                {
                    Abort("Expected !=, got !" + Peek());
                }
            }
            else if (CurChar == '\"')
            {
                //Get characters between quotations
                NextChar();
                _startPos = CurPos;

                while (CurChar != '\"')
                {
                    //Don't allow special characters in the string. No escape characters, newlines, tabs, or %.
                    if (CurChar == '\r' || CurChar == '\n' || CurChar == '\t' || CurChar == '\\' || CurChar == '%')
                    {
                        Abort("Illegal character in string");
                    }
                    NextChar();
                }

                _substringLength = CurPos - _startPos +1;
                _tokText = Source.Substring(_startPos, _substringLength); // Get the substring
                _token = new Token(_tokText, TokenType.STRING);
            }
            else if (Char.IsDigit(CurChar))
            {
                //Leading character is a digit, so this must be a number
                //Get all consecutive digits and decimal if there is one
                _startPos = CurPos;
                while (Char.IsDigit(Peek()))
                {
                    NextChar();
                }
                if (Peek() == '.') //Decimal number
                {
                    NextChar();
                    
                    //Must have at least one digit after decimal
                    if (!Char.IsDigit(Peek()))
                    {
                        Abort("Illegal character in number");
                    }
                    while (Char.IsDigit(Peek()))
                    {
                        NextChar();
                    }
                }

                _substringLength = CurPos - _startPos +1;
                _tokText = Source.Substring(_startPos, _substringLength); //Get substring
                _token = new Token(_tokText, TokenType.NUMBER);
            }
            else if (Char.IsLetter(CurChar))
            {
                //Leading character is a letter, so this must be an identifier or a keyword
                //Get all consecutive alpha numeric characters
                _startPos = CurPos;
                while (Char.IsLetterOrDigit(Peek()))
                {
                    NextChar();
                }
                
                //Check if the token is in the list of keywords
                _substringLength = CurPos - (_startPos+1);
                _tokText = Source.Substring(_startPos, _substringLength+2); //Get substring
                _keyword = Token.checkIfKeyword(_tokText);

                if (_keyword == TokenType.NULL) //Identifier
                {
                    _token = new Token(_tokText, TokenType.IDENT);
                }
                else //Keyword
                {
                    _token = new Token(_tokText, _keyword);
                }
            }
            else if (CurChar == '\n')
            {
                _token = new Token(Char.ToString(CurChar), TokenType.NEWLINE);
            }

            else if (CurChar == '\0')
            {
                _token = new Token(Char.ToString(CurChar), TokenType.EOF);
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
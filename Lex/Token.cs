using System;

namespace rin
{
    //Token contains the original text and the type of token.
    class Token
    {
        public string _text;
        public TokenType _kind;

        public Token(string tokenText, TokenType tokenKind)
        {
            _text = tokenText;
            _kind = tokenKind;
        }

        public static TokenType checkIfKeyword(string tokenText)
        {
            foreach (var kind in Enum.GetValues(typeof(TokenType)))
            {
                // System.Console.WriteLine("int is " + (int)kind );
                // System.Console.WriteLine("string is " + kind.ToString());
                if (kind.ToString() == tokenText && (int)kind >= 100 && (int)kind <200)
                {
                    return (TokenType)kind;
                }
            }
            return TokenType.NULL;
        }
    }
}

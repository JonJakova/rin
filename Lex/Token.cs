namespace rin
{
    //Token contains the original text and the type of token.
    class Token
    {
        public char _text;
        public TokenType _kind;

        public Token(char tokenText, TokenType tokenKind)
        {
            _text = tokenText;
            _kind = tokenKind;
        }
    }
}

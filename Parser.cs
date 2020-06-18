using System.Data;

namespace rin
{
    //Parser object keeps track of current token and checks if the code matches the grammar
    class Parser
    {
        private readonly Lexer _lex;
        private Token _curToken;
        private Token _peekToken;

        public Parser(Lexer lex)
        {
            this._lex = lex;
            this._curToken = null;
            this._peekToken = null;
            NextToken();
            NextToken();
        }

        //Return true if the current token matches
        public bool CheckToken(TokenType kind)
        {
            return kind == _curToken.kind;
        }

        //Return true if the next token matches
        public bool CheckPeek(TokenType kind)
        {
            return kind == _peekToken.kind;
        }

        //Try to match current token. If not, error. Advances the current token
        public void Match(TokenType kind)
        {
            if (!CheckToken(kind))
            {
                Abort("Expected "+ kind.ToString() + ", got " + _curToken.kind.ToString());
            }
            NextToken();
        }

        //Advances the current token
        public void NextToken()
        {
            _curToken = _peekToken;
            _peekToken = _lex.GetToken();
            //No need to worry about passing the EOF, lexer handles that
        }

        public void Abort(string message)
        {
            throw new InvalidExpressionException("Error. " + message);
        }
    }
}

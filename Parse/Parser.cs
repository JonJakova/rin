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

        //Will got to partial class
        // One of the following statements...
        public void Statement()
        {
            // Check the first token to see what kind of statement this is.

            // "PRINT" (expression | string)
            if (CheckToken(TokenType.PRINT))
            {
                System.Console.WriteLine("STATEMENT-PRINT");
                NextToken();

                if (CheckToken(TokenType.STRING))
                {
                    // Simple string
                    NextToken();
                }
                else
                {
                    // Expect an expression
                    // Expression();
                }
            }

            // "IF" comparison "THEN" {statement} "ENDIF"
            else if (CheckToken(TokenType.IF))
            {
                System.Console.WriteLine("STATEMENT-IF");
                NextToken();
                // Comparison();

                Match(TokenType.THEN);
                Nl();

                while (!CheckToken(TokenType.ENDIF))
                {
                    Statement();
                }

                Match(TokenType.ENDIF);
            }

            //
            else if (CheckToken(TokenType.WHILE))
            {
                System.Console.WriteLine("STATEMENT-WHILE");
                NextToken();
                // Comparison();

                Match(TokenType.REPEAT);
                Nl();

                //
                while (!CheckToken(TokenType.ENDWHILE))
                {
                    
                }
            }

            // Newline
            Nl();
        }

        public void Nl()
        {
            System.Console.WriteLine("NEWLINE");

            // Require at least one newline
            Match(TokenType.NEWLINE);
            // But we will allow extra newlines too
            while (CheckToken(TokenType.NEWLINE))
            {
                NextToken();
            }
        }

        // Production rules
        // program ::= {statement}
        public void Program()
        {
            System.Console.WriteLine("PROGRAM");

            // Parse all the statements in the program
            while (!CheckToken(TokenType.EOF))
            {
                Statement();
            }
        }
    }
}

namespace rin
{
    partial class Parser
    {
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
                    Expression();
                }
            }

            // "IF" comparison "THEN" {statement} "ENDIF"
            else if (CheckToken(TokenType.IF))
            {
                System.Console.WriteLine("STATEMENT-IF");
                NextToken();
                Comparison();

                Match(TokenType.THEN);
                Nl();

                while (!CheckToken(TokenType.ENDIF))
                {
                    Statement();
                }

                Match(TokenType.ENDIF);
            }

            // "WHILE" comparison "REPEAT" {statement} "ENDWHILE"
            else if (CheckToken(TokenType.WHILE))
            {
                System.Console.WriteLine("STATEMENT-WHILE");
                NextToken();
                Comparison();

                Match(TokenType.REPEAT);
                Nl();

                // Zero or more statements in the loop body
                while (!CheckToken(TokenType.ENDWHILE))
                {
                    Statement();
                }

                Match(TokenType.ENDWHILE);
            }

            // "LABEL" indet
            else if (CheckToken(TokenType.LABEL))
            {
                System.Console.WriteLine("STATEMENT-LABEL");
                NextToken();

                //Make sure this label doesn't already exist
                if (_labelsDeclared.Contains(_curToken.text))
                {
                    Abort("Label already exists: " + _curToken.text);
                }
                _labelsDeclared.Add(_curToken.text);

                Match(TokenType.IDENT);
            }

            // "GOTO" ident
            else if (CheckToken(TokenType.GOTO))
            {
                System.Console.WriteLine("STATEMENT-IDENT");
                NextToken();
                _labelsGotoed.Add(_curToken.text);
                Match(TokenType.IDENT);
            }

            // "LET" ident "=" expression
            else if (CheckToken(TokenType.LET))
            {
                System.Console.WriteLine("STATEMENT-LET");
                NextToken();

                //
                if (!_symbols.Contains(_curToken.text))
                {
                    _symbols.Add(_curToken.text);
                }

                Match(TokenType.IDENT);
                Match(TokenType.EQ);
                
                Expression();
            }

            // "INPUT" ident
            else if (CheckToken(TokenType.INPUT))
            {
                System.Console.WriteLine("STATEMENT-INPUT");
                NextToken();
                
                //If variable doesn't already exist, declare it
                if (!_symbols.Contains(_curToken.text))
                {
                    _symbols.Add(_curToken.text);
                }

                Match(TokenType.IDENT);
            }

            // This is not a valid statement. Error!
            else
            {
                Abort("Invalid statement at "+ _curToken.text + " (" + _curToken.kind.ToString() + ")");
            }

            // Newline
            Nl();
        }
    }
}
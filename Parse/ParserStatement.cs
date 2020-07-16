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
                NextToken();

                if (CheckToken(TokenType.STRING))
                {
                    // Simple string, so print it
                    _emit.EmitLine("printf(\"" + _curToken.text + "\\n\");");
                    NextToken();
                }
                else
                {
                    // Expect an expression and print the result as a float
                    _emit.Emit("printf(\"%" + ".2f\\n\", (float)(");
                    Expression();
                    _emit.EmitLine("));");
                }
            }

            // "IF" comparison "THEN" {statement} "ENDIF"
            else if (CheckToken(TokenType.IF))
            {
                NextToken();
                _emit.Emit("if(");
                Comparison();

                Match(TokenType.THEN);
                Nl();
                _emit.EmitLine("){");

                // Zero or more statements in the body
                while (!CheckToken(TokenType.ENDIF))
                {
                    Statement();
                }

                Match(TokenType.ENDIF);
                _emit.EmitLine("}");
            }

            // "WHILE" comparison "REPEAT" {statement} "ENDWHILE"
            else if (CheckToken(TokenType.WHILE))
            {
                NextToken();
                _emit.Emit("while(");
                Comparison();

                Match(TokenType.REPEAT);
                Nl();
                _emit.EmitLine("){");

                // Zero or more statements in the loop body
                while (!CheckToken(TokenType.ENDWHILE))
                {
                    Statement();
                }

                Match(TokenType.ENDWHILE);
                _emit.EmitLine("}");
            }

            // "LABEL" indet
            else if (CheckToken(TokenType.LABEL))
            {
                NextToken();

                //Make sure this label doesn't already exist
                if (_labelsDeclared.Contains(_curToken.text))
                {
                    Abort("Label already exists: " + _curToken.text);
                }
                _labelsDeclared.Add(_curToken.text);

                _emit.EmitLine(_curToken.text + ":");
                Match(TokenType.IDENT);
            }

            // "GOTO" ident
            else if (CheckToken(TokenType.GOTO))
            {
                NextToken();
                _labelsGotoed.Add(_curToken.text);
                _emit.EmitLine("goto " + _curToken.text + ";");
                Match(TokenType.IDENT);
            }

            // "LET" ident "=" expression
            else if (CheckToken(TokenType.LET))
            {
                NextToken();

                // Check if ident exists in symbol table. If not, declare it
                if (!_symbols.Contains(_curToken.text))
                {
                    _symbols.Add(_curToken.text);
                    _emit.HeaderLine("float " + _curToken.text + ";");
                }

                _emit.Emit(_curToken.text + " = ");
                Match(TokenType.IDENT);
                Match(TokenType.EQ);
                
                Expression();
                _emit.EmitLine(";");
            }

            // "INPUT" ident
            else if (CheckToken(TokenType.INPUT))
            {
                NextToken();
                
                //If variable doesn't already exist, declare it
                if (!_symbols.Contains(_curToken.text))
                {
                    _symbols.Add(_curToken.text);
                    _emit.HeaderLine("float " + _curToken.text + ";");
                }

                // Emit scanf but also validate the input. If invalid, set the variable to 0 and clear the input.
                _emit.EmitLine("if(0 == scanf(\"%" + "f\", &" + _curToken.text + ")) {");
                _emit.EmitLine(_curToken.text + " =0;");
                _emit.Emit("scanf(\"%");
                _emit.EmitLine("*s\");");
                _emit.EmitLine("}");
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
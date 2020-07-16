using System.Collections.Generic;
using System.Data;

namespace rin
{
    //Parser object keeps track of current token and checks if the code matches the grammar
    partial class Parser
    {
        private readonly Lexer _lex;
        private readonly Emitter _emit;
        private Token _curToken;
        private Token _peekToken;
        private List<string> _symbols; // Variables declared so far
        private List<string> _labelsDeclared; // Labels declared so far
        private List<string> _labelsGotoed; // Labels goto'ed so far

        public Parser(Lexer lex, Emitter emit)
        {
            this._lex = lex;
            this._emit = emit;

            _symbols = new List<string>();
            _labelsDeclared = new List<string>();
            _labelsGotoed = new List<string>();

            this._curToken = null;
            this._peekToken = null;
            NextToken();
            NextToken(); // Call this twice to initialize current and peek.
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

        // comparison ::= expression (("==" | "!=" | ">" | ">=" | "<" | "<=") expression)+
        public void Comparison()
        {
            Expression();
            // Must be at least one comparison operator and another expression
            if (IsComparisonOperator())
            {
                _emit.Emit(_curToken.text);
                NextToken();
                Expression();
            }

            // Can have 0 or more comparison operator and expressions
            while (IsComparisonOperator())
            {
                _emit.Emit(_curToken.text);
                NextToken();
                Expression();
            }
        }

        public bool IsComparisonOperator()
        {
            return CheckToken(TokenType.GT) 
            || CheckToken(TokenType.GTEQ)
            || CheckToken(TokenType.LT)
            || CheckToken(TokenType.LTEQ)
            || CheckToken(TokenType.EQEQ)
            || CheckToken(TokenType.NOTEQ);
        }

        // expression ::= term {( "-" | "+" ) term}
        public void Expression()
        {
            Term();
            // Can have 0 or more +/- and expressions
            while (CheckToken(TokenType.PLUS) || CheckToken(TokenType.MINUS))
            {
                _emit.Emit(_curToken.text);
                NextToken();
                Term();
            }
        }

        // term ::= unary {( "/" | "*" ) unary}
        public void Term()
        {
            Unary();
            // Can have 0 or more *// and expressions
            while (CheckToken(TokenType.ASTERISK) || CheckToken(TokenType.SLASH))
            {
                _emit.Emit(_curToken.text);
                NextToken();
                Unary();
            }
        }

        // unary ::= ["+" | "-"] primary
        public void Unary()
        {
            // Optional unary +/-
            if (CheckToken(TokenType.PLUS) || CheckToken(TokenType.MINUS))
            {
                _emit.Emit(_curToken.text);
                NextToken();
            }
            Primary();
        }

        // primary ::= number | ident
        public void Primary()
        {
            if (CheckToken(TokenType.NUMBER))
            {
                _emit.Emit(_curToken.text);
                NextToken();
            }
            else if (CheckToken(TokenType.IDENT))
            {
                // Ensure the variable already exists
                if (!_symbols.Contains(_curToken.text))
                {
                    Abort("Referencing variable before assignment: " + _curToken.text);
                }

                _emit.Emit(_curToken.text);
                NextToken();
            }
            else
            {
                // Error
                Abort("Unexpected token at "+ _curToken.text);
            }
        }

        public void Nl()
        {
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
            _emit.HeaderLine("#include <stdio.h>");
            _emit.HeaderLine("int main(void){");

            // Since some newlines are required in our grammar, need to skip the excess
            while (CheckToken(TokenType.NEWLINE))
            {
                NextToken();
            }

            // Parse all the statements in the program
            while (!CheckToken(TokenType.EOF))
            {
                Statement();
            }

            // Wraps things up
            _emit.EmitLine("return 0;");
            _emit.EmitLine("}");

            //Check that each label referenced in a GOTO is declared
            foreach (var label in _labelsGotoed)
            {
                if (!_labelsDeclared.Contains(label))
                {
                    Abort("Attempting to GOTO to undeclared label: " + label);
                }
            }
        }
    }
}
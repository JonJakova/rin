using System;

namespace rin
{
    class Program
    {
        static void Main(string[] args)
        {
            String input = "IF+-123 foo*THEN/";
            var lexer = new Lexer(input);
            var token = lexer.GetToken();

            while (token._kind != TokenType.EOF)
            {
                System.Console.WriteLine(token._kind);
                token = lexer.GetToken();
            }
        }
    }
}

using System;

namespace rin
{
    class Program
    {
        static void Main(string[] args)
        {
            String input = "LET foobar = 123";
            var lexer = new Lexer(input);

            while (lexer.Peek() != '\0')
            {
                System.Console.WriteLine(lexer.CurChar);
                lexer.NextChar();
            }
        }
    }
}

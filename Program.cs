﻿using System;
using System.IO;

namespace rin
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Rin compiler");

            string input;

            if (args.Length != 1)
            {
                throw new ArgumentNullException("Enter file path");
            }

            using (StreamReader reader = new StreamReader(args[0]))
            {
                input = reader.ReadToEnd();
            }

            //Initialize the lexer and parser
            var lexer = new Lexer(input);
            var emitter = new Emitter("out.c");
            var parser = new Parser(lexer, emitter);

            parser.Program(); // Start the parser
            emitter.WriteFile(); // Write the output to file
            System.Console.WriteLine("Compiling completed");
        }
    }
}

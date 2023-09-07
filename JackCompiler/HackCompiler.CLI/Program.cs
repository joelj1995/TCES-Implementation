// See https://aka.ms/new-console-template for more information
using JackCompiler.Lib;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"Compiling {args[0]}");

        var compiler = new JackCompiler.Lib.JackCompiler();
        compiler.Compile(args[0]);

        Console.WriteLine("Done.");
    }
}
using Assembler.Logic;

internal class Program
{
    private static string Usage = "Usage:\n\tAssembler <Prog.asm>";

    private static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Invalid arugments supplied.");
            Console.WriteLine(Usage);
            return;
        }

        var inputFile = args[0];

        if (!File.Exists(inputFile)) 
        {
            Console.WriteLine("Specified assembly file not found.");
            return;
        }

        if (!inputFile.EndsWith(".asm"))
        {
            Console.WriteLine("Input file is not in the right format");
        }

        var outputFile = inputFile.Replace(".asm", ".hack");

        var assembler = AssemblerFactory.CreateAssembler("Hack");

        using (var reader = new StreamReader(inputFile))
        using (var writer = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
        {
            var assembledBinary = assembler.assemble(reader);
            writer.Write(assembledBinary);
        }
    }
}
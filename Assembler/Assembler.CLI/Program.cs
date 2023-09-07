using Assembler.Logic;
using System.Text;

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

        var assembler = AssemblerFactory.CreateAssembler("Hack");

        var files = Directory.EnumerateFiles(args[0]);
        using (var writer = new BinaryWriter(File.Open(Path.Join(args[0], "Program.hack"), FileMode.Create)))
        {
            var fileQueue = files.Where(f => f.EndsWith(".asm")).OrderBy(FilePriority).ThenBy(f => f);
            foreach (var file in fileQueue)
            {
                using (var reader = new StreamReader(file))
                {
                    Console.WriteLine($"Processing symbols for {file}");
                    assembler.BuildSymbolTable(reader);
                }
            }
            using (var symbolsWriter = new BinaryWriter(File.Open(Path.Join(args[0], "Symbols.pdb"), FileMode.Create)))
            {
                var data = Encoding.ASCII.GetBytes(assembler.DumpSymbolTable());
                symbolsWriter.Write(data);
            }
            foreach (var file in fileQueue)
            {
                using (var reader = new StreamReader(file))
                {
                    Console.WriteLine($"Assembling {file}");
                    var assembledBinary = assembler.assemble(reader);
                    writer.Write(assembledBinary);
                }
            }
            using (var symbolsWriter = new BinaryWriter(File.Open(Path.Join(args[0], "Symbols.pdb"), FileMode.Create)))
            {
                var data = Encoding.ASCII.GetBytes(assembler.DumpSymbolTable());
                symbolsWriter.Write(data);
            }
        }
    }

    private static int FilePriority(string filePath)
    {
        if (Path.GetFileName(filePath).Equals("Sys.asm")) return 0;
        return 1;
    }
}
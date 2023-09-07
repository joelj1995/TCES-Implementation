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

        var assembler = AssemblerFactory.CreateAssembler("Hack");

        var files = Directory.EnumerateFiles(args[0]);
        using (var writer = new BinaryWriter(File.Open(Path.Join(args[0], "Program.hack"), FileMode.Create)))
        {
            foreach (var file in files)
            {
                if (!file.EndsWith(".asm")) continue;
                using (var reader = new StreamReader(file))
                {
                    Console.WriteLine($"Assembling {file}");
                    var assembledBinary = assembler.assemble(reader);
                    writer.Write(assembledBinary);
                }
            }
        }
    }
}
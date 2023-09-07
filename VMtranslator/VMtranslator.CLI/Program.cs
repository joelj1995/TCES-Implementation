// See https://aka.ms/new-console-template for more information
using VMtranslator.Core;
using VMtranslator.Core.Interface;

internal class Program
{
    private static void Main(string[] args)
    {
        var files = Directory.EnumerateFiles(args[0]);
        foreach (var file in files)
        {
            if (!file.EndsWith(".vm")) continue;
            Console.WriteLine($"Translating {file}");
            TranslateFile(file);
        }
    }

    private static void TranslateFile(string progamFile)
    {
        var programName = Path.GetFileName(progamFile).Replace(".vm", "");
        var outputFileName = progamFile.Replace(".vm", ".asm");
        using (var fileWriter = File.Open(outputFileName, FileMode.OpenOrCreate))
        using (var writer = new StreamWriter(fileWriter))
        using (var reader = new StreamReader(progamFile))
        {
            var translator = translatorFactory.create(writer);
            translator.translate(programName, reader);
        }
    }

    private static HackTranslatorFactory translatorFactory = new HackTranslatorFactory();
}
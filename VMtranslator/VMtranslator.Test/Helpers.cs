using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core;
using VMtranslator.Core.Interface;

namespace VMtranslator.Test
{
    internal static class Helpers
    {
        public static string NewLine => Environment.NewLine;

        public static StreamReader GetStreamReaderForString(string input)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            var stream = new MemoryStream(byteArray);
            return new StreamReader(stream);
        }

        public static string TryTranslate(string sampleProgramName)
        {
            var inputPath = Path.Join("Samples", $"{sampleProgramName}.vm");
            return TranslateFiles(new [] { inputPath });
        }

        public static string TryTranslateMany(string subPath)
        {
            subPath = Path.Join("Samples", subPath);
            var files = Directory.EnumerateFiles(subPath);
            return TranslateFiles(files);
        }

        private static string TranslateFiles(IEnumerable<string> paths)
        {
            var result = String.Empty;
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var translator = translatorFactory.create(writer);
                foreach (var progamFile in paths)
                {
                    TranslateFile(translator, progamFile);
                }
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                result = resultReader.ReadToEnd();
            }
            return result;
        }

        private static void TranslateFile(ITranslator translator, string progamFile)
        {
            var programName = Path.GetFileName(progamFile).Replace(".vm", "");
            using (var reader = new StreamReader(progamFile))
            {
                translator.translate(programName, reader);
            }
        }

        private static HackTranslatorFactory translatorFactory = new HackTranslatorFactory();
    }
}

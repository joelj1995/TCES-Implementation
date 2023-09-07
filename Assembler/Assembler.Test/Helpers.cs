using Assembler.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Test
{
    public static class Helpers
    {
        static string TempPath = "C:\\DebugTmp";

        public static StreamReader GetStreamReaderForString(string input)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            var stream = new MemoryStream(byteArray);
            return new StreamReader(stream);
        }

        public static void TestAssemble(string programName, IAssembler assembler)
        {
            var inputPath = $"./Samples/{programName}.asm";
            var comparePath = $"./Samples/{programName}.hack";
            using (var reader = new StreamReader(inputPath))
            {
                var assembledBinary = assembler.assemble(reader);
                var targetBinary = File.ReadAllBytes(comparePath);
                if (Directory.Exists(TempPath))
                {
                    using (var writer = new BinaryWriter(File.Open($"{TempPath}\\{programName}.hack", FileMode.Create)))
                    {
                        writer.Write(assembledBinary);
                    }
                }
                CollectionAssert.AreEqual(assembledBinary, targetBinary);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Hack;

namespace VMtranslator.Test.Hack
{
    internal abstract class TestHackCodeWriter
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        protected HackCodeWriter codeWriter = null;
        protected MemoryStream stream = null;
        protected StreamWriter writer = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        [SetUp]
        public void Setup()
        {
            stream = new MemoryStream();
            writer = new StreamWriter(stream);
            codeWriter = new HackCodeWriter(writer);
        }

        [TearDown]
        public void Teardown()
        {
            writer.Dispose();
            stream.Dispose();
        }

        protected string cleanString(string input)
        {
            var sb = new StringBuilder();
            foreach (var line in input.Split("\n"))
            {
                var trimmedLine = line.Trim();
                if (!trimmedLine.StartsWith("//") && !String.IsNullOrWhiteSpace(trimmedLine))
                    sb.AppendLine(trimmedLine);
            }
            return sb.ToString().Trim();
        }

        protected string GetWriterText()
        {
            using (var reader = new StreamReader(stream))
            {
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var result = reader.ReadToEnd().Trim();
                return cleanString(result);
            }
        }

        protected string buildString(string[] lines)
        {
            var sb = new StringBuilder();
            foreach (var line in lines) sb.AppendLine(line);
            return sb.ToString().Trim();
        }
    }
}

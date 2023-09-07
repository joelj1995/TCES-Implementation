using JackCompiler.Lib.Jack;
using Microsoft.XmlDiffPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace JackCompiler.Lib.Test
{
    internal static class Helpers
    {
        public static StreamReader GetStreamReaderForString(string input)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            var stream = new MemoryStream(byteArray);
            return new StreamReader(stream);
        }

        public static void TestAnalyzeSample(string samplePath)
        {
            var sampleJackPath = Path.Join("Samples", samplePath) + ".jack";
            var sampleExpectedOutputPath = Path.Join("Samples", samplePath) + ".xml";
            using (var inputStream = new StreamReader(sampleJackPath))
            using (var expectedStream = new StreamReader(sampleExpectedOutputPath))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (var writer = new StreamWriter(memoryStreamForWriter))
            {
                var analyzer = ProcessorFactory.CreateForXmlOutput(inputStream, writer);
                analyzer.Process();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                AssertXmlEqual(expectedStream , resultReader);
            }
        }

        public static void CompareTokens(string samplePath)
        {
            var sampleJackPath = Path.Join("Samples", samplePath) + ".jack";
            var sampleExpectedOutputPath = Path.Join("Samples", samplePath) + "T.xml";
            string result = String.Empty;
            using (var expectedStream = new StreamReader(sampleExpectedOutputPath))
            using (var inputStream = new StreamReader(sampleJackPath))
            {
                result = BuildTokenXML(inputStream);
                Helpers.AssertXmlEqual(expectedStream, result.ToString());
            }
        }

        public static void CompareTokens(string input, string expected)
        {
            string result = String.Empty;
            using (var inputStream = Helpers.GetStreamReaderForString(input))
            {
                result = BuildTokenXML(inputStream);
            }
            Helpers.AssertXmlEqual(expected, result.ToString());
        }

        private static string BuildTokenXML(StreamReader inputStream)
        {
            var result = new StringBuilder();
            result.AppendLine("<tokens>");
            var tokenizer = new JackTokenizer(inputStream);
            while (tokenizer.HasMoreTokens())
            {
                tokenizer.Advance();
                result.AppendLine(tokenizer.ToXML());
            }
            result.AppendLine("</tokens>");
            return result.ToString();
        }

        public static void AssertXmlEqual(string xml1, string xml2)
        {
            using (var xml1StreamReader = GetStreamReaderForString(xml1))
            using (var xml2StreamReader = GetStreamReaderForString(xml2))
            {
                AssertXmlEqual(xml1StreamReader, xml2StreamReader);
            }
        }

        public static void AssertXmlEqual(StreamReader xml1, string xml2)
        {
            using (var xml2StreamReader = GetStreamReaderForString(xml2))
            {
                AssertXmlEqual(xml1, xml2StreamReader);
            }
        }

        public static void AssertXmlEqual(string xml1, StreamReader xml2)
        {
            using (var xml1StreamReader = GetStreamReaderForString(xml1))
            {
                AssertXmlEqual(xml1StreamReader, xml2);
            }
        }

        public static void AssertXmlEqual(StreamReader xml1, StreamReader xml2)
        {
            XmlDiff xmldiff = new XmlDiff(
                XmlDiffOptions.IgnoreNamespaces |
                XmlDiffOptions.IgnorePrefixes);
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                NewLineOnAttributes = true
            };
            using (var xml1Reader = XmlReader.Create(xml1))
            using (var xml2Reader = XmlReader.Create(xml2))
            using (var memoryStreamForWriter = new MemoryStream())
            using (var resultReader = new StreamReader(memoryStreamForWriter))
            using (XmlWriter diffGramWriter = XmlWriter.Create(memoryStreamForWriter, settings))
            {
                if (xml1Reader is null) throw new ArgumentNullException(nameof(xml1Reader));
                if (xml2Reader is null) throw new ArgumentNullException(nameof(xml2Reader));
                try
                {
                    memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                    bool xmlAreEqual = xmldiff.Compare(xml1Reader, xml2Reader, diffGramWriter);
                    if (!xmlAreEqual)
                    {
                        xml2.BaseStream.Seek(0, SeekOrigin.Begin);
                        memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                        var actual = xml2.ReadToEnd();
                        var diff = resultReader.ReadToEnd();
                        Assert.Fail($"XML does not match.\nGot:\n{actual}\nDiff:\n{diff}");
                    }
                }
                catch (XmlException e)
                {
                    xml1.BaseStream.Seek(0, SeekOrigin.Begin);
                    xml2.BaseStream.Seek(0, SeekOrigin.Begin);
                    var expected = xml1.ReadToEnd();
                    var actual = xml2.ReadToEnd();
                    Assert.Fail($"XML is not valid.\nActual:\n{actual}\nExpected:\n{expected}\n");
                }
            }
        }
    }
}

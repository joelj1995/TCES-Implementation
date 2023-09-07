using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test
{
    internal class TestJackCompiler
    {
        [SetUp]
        public void SetUp()
        {
            compiler = new JackCompiler();
        }


        [Test]
        public void TestSeven()
        {
            CompileAndCompare("CompilerSamples\\Seven");
        }

        [Test]
        public void TestConvertToBin()
        {
            CompileAndCompare("CompilerSamples\\ConvertToBin");
        }

        [Test]
        public void TestSquare()
        {
            CompileAndCompare("CompilerSamples\\Square");
        }

        [Test]
        public void TestAverage()
        {
            CompileAndCompare("CompilerSamples\\Average");
        }

        [Test]
        public void TestPong()
        {
            CompileAndCompare("CompilerSamples\\Pong");
        }

        [Test]
        public void TestComplexArrays()
        {
            CompileAndCompare("CompilerSamples\\ComplexArrays");
        }

        private void CompileAndCompare(string path)
        {
            compiler.Compile(path);
            foreach (var file in Directory.GetFiles(path))
            {
                if (!file.EndsWith(".vm")) continue;
                if (file.Split('.')[0].EndsWith("_")) continue;
                var className = file.Split('.')[0];
                var pathForActual = $"{className}.vm";
                var pathForExpected = $"{className}_.vm";
                using (var fileReaderForActual = File.OpenRead(pathForActual))
                using (var fileReaderForExpected = File.OpenRead(pathForExpected))
                using (var inputStreamForActual = new StreamReader(fileReaderForActual))
                using (var inputStreamForExpected = new StreamReader(fileReaderForExpected))
                {
                    var actual = inputStreamForActual.ReadToEnd();
                    var expected = inputStreamForExpected.ReadToEnd();
                    Assert.That(expected, Is.EqualTo(actual), className);
                }
            }
        }

        private ICompiler compiler;
    }
}

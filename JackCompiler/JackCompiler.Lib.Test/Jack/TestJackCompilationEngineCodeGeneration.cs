using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Test.Jack
{
    internal class TestJackCompilationEngineCodeGeneration
    {
        [SetUp]
        public void SetUp()
        {
            memoryStreamForWriter = new MemoryStream();
            resultReader = new StreamReader(memoryStreamForWriter);
            writer = new StreamWriter(memoryStreamForWriter);
            toDispose = new CompositeDisposable(new IDisposable[] { memoryStreamForWriter, resultReader, writer });
        }

        [TearDown]
        public void TearDown()
        {
            toDispose.Dispose();
        }

        [Test]
        public void TestSevenMain()
        {
            var input = @"
                class Main {

                   function void main() {
                       do Output.printInt(1 + (2 * 3));
                       return;
                   }

                }
            ";
            var expected = @"
                function Main.main 0
                push constant 1
                push constant 2
                push constant 3
                call Math.multiply 2
                add
                call Output.printInt 1
                pop temp 0
                push constant 0
                return
            ";
            AssertOutputMatches(input, expected);
        }

        [Test]
        public void TestConverToBinMain()
        {
            var input = @"
                class Main {
    
                    function void main() {
	                var int result, value;
        
                        do Main.fillMemory(8001, 16, -1); // sets RAM[8001]..RAM[8016] to -1
                        let value = Memory.peek(8000);    // reads a value from RAM[8000]
	                do Main.convert(value);           // perform convertion
    
    	                return;
                    }
    
                    function void convert(int value) {
    	                var int mask, position;
    	                var boolean loop;
    	
    	                let loop = true;
 
    	                while (loop) {
    	                    let position = position + 1;
    	                    let mask = Main.nextMask(mask);
                            do Memory.poke(9000 + position, mask);
    	
    	                    if (~(position > 16)) {
    	
    	                        if (~((value & mask) = 0)) {
    	                            do Memory.poke(8000 + position, 1);
       	                        }
    	                        else {
    	                            do Memory.poke(8000 + position, 0);
      	                        }    
    	                    }
    	                    else {
    	                        let loop = false;
    	                    }
    	                }
    	
    	                return;
                    }
 
                    function int nextMask(int mask) {
    	                if (mask = 0) {
    	                    return 1;
    	                }
    	                else {
	                    return mask * 2;
    	                }
                    }
    
                    function void fillMemory(int startAddress, int length, int value) {
                        while (length > 0) {
                            do Memory.poke(startAddress, value);
                            let length = length - 1;
                            let startAddress = startAddress + 1;
                        }
        
                        return;
                    }
                }

            ";
            var expected = @"
                function Main.main 2
                push constant 8001
                push constant 16
                push constant 1
                neg
                call Main.fillMemory 3
                pop temp 0
                push constant 8000
                call Memory.peek 1
                pop local 1
                push local 1
                call Main.convert 1
                pop temp 0
                push constant 0
                return
                function Main.convert 3
                push constant 0
                not
                pop local 2
                label WHILE_EXP0
                push local 2
                not
                if-goto WHILE_END0
                push local 1
                push constant 1
                add
                pop local 1
                push local 0
                call Main.nextMask 1
                pop local 0
                push constant 9000
                push local 1
                add
                push local 0
                call Memory.poke 2
                pop temp 0
                push local 1
                push constant 16
                gt
                not
                if-goto IF_TRUE0
                goto IF_FALSE0
                label IF_TRUE0
                push argument 0
                push local 0
                and
                push constant 0
                eq
                not
                if-goto IF_TRUE1
                goto IF_FALSE1
                label IF_TRUE1
                push constant 8000
                push local 1
                add
                push constant 1
                call Memory.poke 2
                pop temp 0
                goto IF_END1
                label IF_FALSE1
                push constant 8000
                push local 1
                add
                push constant 0
                call Memory.poke 2
                pop temp 0
                label IF_END1
                goto IF_END0
                label IF_FALSE0
                push constant 0
                pop local 2
                label IF_END0
                goto WHILE_EXP0
                label WHILE_END0
                push constant 0
                return
                function Main.nextMask 0
                push argument 0
                push constant 0
                eq
                if-goto IF_TRUE0
                goto IF_FALSE0
                label IF_TRUE0
                push constant 1
                return
                goto IF_END0
                label IF_FALSE0
                push argument 0
                push constant 2
                call Math.multiply 2
                return
                label IF_END0
                function Main.fillMemory 0
                label WHILE_EXP0
                push argument 1
                push constant 0
                gt
                not
                if-goto WHILE_END0
                push argument 0
                push argument 2
                call Memory.poke 2
                pop temp 0
                push argument 1
                push constant 1
                sub
                pop argument 1
                push argument 0
                push constant 1
                add
                pop argument 0
                goto WHILE_EXP0
                label WHILE_END0
                push constant 0
                return
            ";
            AssertOutputMatches(input, expected);
        }

        private void AssertOutputMatches(string jackInput, string expected)
        {
            using (var inputStream = Helpers.GetStreamReaderForString(jackInput))
            {
                var engine = new JackCompilationEngine(new JackConfiguration(), inputStream, writer);
                engine.CompileClass();
                writer.Flush();
                memoryStreamForWriter.Seek(0, SeekOrigin.Begin);
                var actual = resultReader.ReadToEnd().Trim();
                Assert.That(CleanString(expected), Is.EqualTo(CleanString(actual)));
            }
        }

        private string CleanString(string input)
        {
            var sb = new StringBuilder();
            foreach (var line in input.Split(Environment.NewLine))
            {
                if (String.IsNullOrWhiteSpace(line)) continue;
                sb.AppendLine(line.Trim());
            }
            return sb.ToString();
        }

        private MemoryStream memoryStreamForWriter;
        private StreamReader resultReader;
        private StreamWriter writer;
        private IDisposable toDispose;
    }
}

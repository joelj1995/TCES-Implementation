using JackCompiler.Lib.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib.Jack
{
    internal class JackVMWriter : IVMWriter, IJackCompilationState
    {
        public JackVMWriter(StreamWriter outputStream) 
        {
            this.outputStream = outputStream;
        }

        public void WriteArithmetic(Command command)
        {
            switch (command) 
            {
                case Command.ADD: outputStream.WriteLine("add"); stackLength -= 1; break;
                case Command.SUB: outputStream.WriteLine("sub"); stackLength -= 1; break;
                case Command.NEG: outputStream.WriteLine("neg"); break;
                case Command.EQ: outputStream.WriteLine("eq"); stackLength -= 1; break;
                case Command.GT: outputStream.WriteLine("gt"); stackLength -= 1; break;
                case Command.LT: outputStream.WriteLine("lt"); stackLength -= 1; break;
                case Command.AND: outputStream.WriteLine("and"); stackLength -= 1; break;
                case Command.OR: outputStream.WriteLine("or"); stackLength -= 1; break;
                case Command.NOT: outputStream.WriteLine("not"); break;
                default: throw new NotImplementedException(command.ToString());
            }
        }

        public void WriteCall(string name, int nArgs)
        {
            outputStream.WriteLine($"call {name} {nArgs}");
            stackLength -= (nArgs - 1);
        }

        public void WriteFunction(string name, int nLocals)
        {
            outputStream.WriteLine($"function {name} {nLocals}");
        }

        public void WriteGoto(string label)
        {
            outputStream.WriteLine($"goto {label}");
        }

        public void WriteIf(string label)
        {
            outputStream.WriteLine($"if-goto {label}");
            stackLength--;
        }

        public void WriteLabel(string label)
        {
            outputStream.WriteLine($"label {label}");
        }

        public void WritePop(Segment segment, int index)
        {
            outputStream.WriteLine($"pop {SegmentName(segment)} {index}");
            stackLength--;
        }

        public void WritePush(Segment segment, int index)
        {
            outputStream.WriteLine($"push {SegmentName(segment)} {index}");
            stackLength++;
        }

        public void WriteReturn()
        {
            outputStream.WriteLine("return");
        }

        public void NextRoutine()
        {
            stackLength = 0;
        }

        public int StackLength => stackLength;

        private string SegmentName(Segment segment)
        {
            switch (segment)
            {
                case Segment.ARG: return "argument";
                case Segment.CONST: return "constant";
                case Segment.LOCAL: return "local";
                case Segment.POINTER: return "pointer";
                case Segment.STATIC: return "static";
                case Segment.TEMP: return "temp";
                case Segment.THIS: return "this";
                case Segment.THAT: return "that";
                default: throw new NotImplementedException(segment.ToString());
            }
        }

        private readonly StreamWriter outputStream;
        private int stackLength = 0;
    }
}

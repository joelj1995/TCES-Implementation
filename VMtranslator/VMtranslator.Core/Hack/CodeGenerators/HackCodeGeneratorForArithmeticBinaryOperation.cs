using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VMtranslator.Core.Hack.HackPlatform;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForArithmeticBinaryOperation : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForArithmeticBinaryOperation(HackCodeWriterContext context, string operation) : base(context)
        {
            this.operation = operation;
        }

        public override HackScript getCode()
        {
            return 
                popOperandsAndAdvanceStack() +
                HackCommand.C("D", $"A{operation}D") +
                HackCommand.A(ReservedRegisters.SP) +
                HackCommand.C("A", "M-1") +
                HackCommand.C("M", "D");
        }

        private readonly string operation;
    }
}

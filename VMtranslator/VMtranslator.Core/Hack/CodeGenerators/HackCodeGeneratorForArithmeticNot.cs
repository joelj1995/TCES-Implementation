using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VMtranslator.Core.Hack.HackPlatform;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForArithmeticNot : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForArithmeticNot(HackCodeWriterContext context) : base(context)
        {
        }

        public override HackScript getCode()
        {
            return
                HackCommand.A(ReservedRegisters.SP) +
                HackCommand.C("A", "M-1") +
                HackCommand.C("D", "!M") +
                HackCommand.C("M", "D");
        }
    }
}

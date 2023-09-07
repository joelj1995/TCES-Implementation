using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VMtranslator.Core.Hack.HackPlatform;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForArithmeticBinaryCompare : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForArithmeticBinaryCompare(HackCodeWriterContext context, string jump) : base(context)
        {
            this.jump = jump;
        }

        public override HackScript getCode()
        {
            return computeBooleanResult(jump);
        }

        private HackScript computeBooleanResult(string jump)
        {
            var yesLocation = location("YES");
            var exitLocation = location("EXIT");
            return popOperandsAndAdvanceStack() +
                HackCommand.C("D", "A-D") +
                HackCommand.A(yesLocation) +
                HackCommand.C(null, "D", jump) +
                HackCommand.A("0") +
                HackCommand.C("D", "A") +
                HackCommand.A(exitLocation) +
                HackCommand.C(null, "0", "JMP") +
                HackCommand.L(yesLocation) +
                HackCommand.A("1") +
                HackCommand.C("A", "-A") +
                HackCommand.C("D", "A") +
                HackCommand.L(exitLocation) +
                HackCommand.A(ReservedRegisters.SP) +
                HackCommand.C("A", "M-1") +
                HackCommand.C("M", "D");
        }

        private readonly string jump;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Interface;
using static VMtranslator.Core.Hack.HackPlatform;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForHeader : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForHeader(HackCodeWriterContext context) : base(context) { }

        public override HackScript getCode()
        {
            var callInitGenerator = new HackCodeGeneratorForFunctionCall(context, VMConstants.EntryMethod, 0);
            return 
                initRam(ReservedRegisters.SP, MemorySegments.Stack.Start.Value) +
                callInitGenerator.compile();
        }

        private HackScript initRam(HackSymbol symbol, int value)
        {
            return
                HackCommand.A(value) +
                HackCommand.C("D", "A") +
                HackCommand.A(symbol) +
                HackCommand.C("M", "D");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static VMtranslator.Core.Hack.HackPlatform;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForStackPopSegment : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForStackPopSegment(HackCodeWriterContext context, HackSymbol segment, int index) : base(context)
        {
            this.segment = segment;
            this.index = index;
        }

        public override HackScript getCode()
        {
            return 
                storeSegmentAddressAtSymbol(segment, index, GeneralRegisters.R13) +
                popStackIntoD() +
                HackCommand.A(GeneralRegisters.R13) +
                HackCommand.C("A", "M") +
                HackCommand.C("M", "D");
        }

        private HackScript storeSegmentAddressAtSymbol(HackSymbol segment, int index, HackSymbol symbol)
        {
            return
                HackCommand.A(index) +
                HackCommand.C("D", "A") +
                HackCommand.A(segment) +
                HackCommand.C("D", "M+D") +
                HackCommand.A(symbol) +
                HackCommand.C("M", "D");
        }

        private int index;
        private HackSymbol segment;
    }
}

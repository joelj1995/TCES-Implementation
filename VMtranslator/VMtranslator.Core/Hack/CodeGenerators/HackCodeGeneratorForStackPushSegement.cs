using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForStackPushSegement : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForStackPushSegement(HackCodeWriterContext context, HackSymbol segment, int index) : base(context)
        {
            this.segment = segment;
            this.index = index;
        }

        public override HackScript getCode()
        {
            return
                HackCommand.A(index) +
                HackCommand.C("D", "A") +
                HackCommand.A(segment) +
                HackCommand.C("A", "M") +
                HackCommand.C("A", "A+D") +
                HackCommand.C("D", "M") +
                pushDOntoStack();
        }

        private int index;
        private HackSymbol segment;
    }
}

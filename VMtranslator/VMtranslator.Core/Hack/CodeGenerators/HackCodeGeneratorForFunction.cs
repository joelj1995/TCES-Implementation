using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForFunction : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForFunction(HackCodeWriterContext context, string functionName, int numLocals) : base(context)
        {
            this.functionName = functionName;
            this.numLocals = numLocals;
        }

        public override HackScript getCode()
        {
            return
                HackCommand.L(functionName) +
                Enumerable.Repeat(initLocal(), numLocals).Sum();
        }

        private HackScript initLocal()
        {
            return
                HackCommand.A(0) +
                HackCommand.C("D", "A") +
                pushDOntoStack();
        }

        private readonly string  functionName;
        private readonly int numLocals;
    }
}

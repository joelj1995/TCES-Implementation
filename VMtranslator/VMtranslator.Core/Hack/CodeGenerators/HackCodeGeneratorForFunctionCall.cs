using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForFunctionCall : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForFunctionCall(HackCodeWriterContext context, string functionName, int args) : base(context)
        {
            this.functionName = functionName;
            this.args = args;
        }

        public override HackScript getCode()
        {
            var returnLocation = location("RET");
            return
                pushReturnAddress(returnLocation) +
                pushSymbolOntoStack(LCL) +
                pushSymbolOntoStack(ARG) +
                pushSymbolOntoStack(THIS) +
                pushSymbolOntoStack(THAT) +
                repositionARG() +
                repositionLCL() +
                HackCommand.A(functionName) +
                HackCommand.C(null, "0", "JMP") +
                HackCommand.L(returnLocation);
        }

        private HackScript pushReturnAddress(HackSymbol returnLocation)
        {
            return
                HackCommand.A(returnLocation) +
                HackCommand.C("D", "A") +
                pushDOntoStack();
        }

        private HackScript pushSymbolOntoStack(HackSymbol symbol)
        {
            return
                loadSymbolIntoD(symbol) +
                pushDOntoStack();
        }

        private HackScript repositionARG()
        {
            return
                HackCommand.A(new HackSymbol(5 + args)) +
                HackCommand.C("D", "A") +
                HackCommand.A(SP) +
                HackCommand.C("D", "M-D") +
                HackCommand.A(ARG) +
                HackCommand.C("M", "D");
        }

        private HackScript repositionLCL()
        {
            return
                HackCommand.A(SP) +
                HackCommand.C("D", "M") +
                HackCommand.A(LCL) +
                HackCommand.C("M", "D");
        }

        private readonly string functionName;
        private readonly int args;
    }
}

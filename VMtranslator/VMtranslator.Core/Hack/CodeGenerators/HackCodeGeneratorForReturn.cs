using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal class HackCodeGeneratorForReturn : AbstractHackCodeGenerator
    {
        public HackCodeGeneratorForReturn(HackCodeWriterContext context) : base(context)
        {
            
        }

        public override HackScript getCode()
        {
            var frame = R13;
            var ret = R14;
            return
                storeLCL(frame) +
                readAndStoreRet(frame, ret) +
                placeReturnValueOnStack() +
                restoreSP() +
                popFrameIntoSymbol(frame, THAT) +
                popFrameIntoSymbol(frame, THIS) +
                popFrameIntoSymbol(frame, ARG) +
                popFrameIntoSymbol(frame, LCL) +
                jumpToReturn(ret);
        }

        private HackScript storeLCL(HackSymbol frame)
        {
            return
                HackCommand.A(LCL) +
                HackCommand.C("D", "M") +
                HackCommand.A(R13) +
                HackCommand.C("M", "D");
        }

        private HackScript readAndStoreRet(HackSymbol frame, HackSymbol ret)
        {
            return
                HackCommand.A(5) +
                HackCommand.C("D", "A") +
                HackCommand.A(frame) +
                HackCommand.C("A", "M-D") +
                HackCommand.C("D", "M") +
                HackCommand.A(ret) +
                HackCommand.C("M", "D");
        }

        private HackScript placeReturnValueOnStack()
        {
            return
                popStackIntoD() +
                HackCommand.A(ARG) +
                HackCommand.C("A", "M") +
                HackCommand.C("M", "D");
        }

        private HackScript restoreSP()
        {
            return
                HackCommand.A(ARG) +
                HackCommand.C("D", "M") +
                HackCommand.A(SP) +
                HackCommand.C("M", "D+1");
        }

        private HackScript popFrameIntoSymbol(HackSymbol frame, HackSymbol symbol)
        {
            return
                HackCommand.A(frame) +
                HackCommand.C("M", "M-1") +
                HackCommand.C("A", "M") + // TODO: can this be removed?
                HackCommand.C("D", "M") +
                HackCommand.A(symbol) +
                HackCommand.C("M", "D");
        }

        private HackScript jumpToReturn(HackSymbol ret)
        {
            return
                HackCommand.A(ret) +
                HackCommand.C("A", "M") +
                HackCommand.C(null, "0", "JMP");
        }
    }
}

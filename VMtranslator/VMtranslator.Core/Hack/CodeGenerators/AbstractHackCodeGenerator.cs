using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static VMtranslator.Core.Hack.HackPlatform;

namespace VMtranslator.Core.Hack.CodeGenerators
{
    internal abstract class AbstractHackCodeGenerator
    {
        protected AbstractHackCodeGenerator(HackCodeWriterContext context)
        {
            this.context = context;
        }

        public HackScript compile()
        {
            return
                HackCommand.CommentF(this.GetType().Name).toScript()
                + getCode();
        }

        public abstract HackScript getCode();

        protected HackSymbol location(string name)
        {
            return new HackSymbol($"{name}_{context.nextSymbolCookie()}");
        }

        protected HackScript popStackIntoD()
        {
            return
                HackCommand.A(ReservedRegisters.SP) +
                HackCommand.C("D", "M-1") +
                HackCommand.C("M", "D") +
                HackCommand.C("A", "D") +
                HackCommand.C("D", "M");
        }

        protected HackScript pushDOntoStack()
        {
            return
                HackCommand.A(ReservedRegisters.SP) +
                HackCommand.C("A", "M") +
                HackCommand.C("M", "D") +
                HackCommand.C("D", "A+1") +
                HackCommand.A(ReservedRegisters.SP) +
                HackCommand.C("M", "D");
        }

        protected HackScript popOperandsAndAdvanceStack()
        {
            return
                HackCommand.A(ReservedRegisters.SP) +
                HackCommand.C("D", "M-1") +
                HackCommand.C("M", "D") +
                HackCommand.C("A", "M") +
                HackCommand.C("D", "M") +
                HackCommand.C("A", "A-1") +
                HackCommand.C("A", "M");
        }

        protected HackScript loadSymbolIntoD(HackSymbol symbol)
        {
            return
                HackCommand.A(symbol) +
                HackCommand.C("A", "M") +
                HackCommand.C("D", "A");
        }

        protected readonly HackCodeWriterContext context;
        protected HackSymbol SP => HackPlatform.ReservedRegisters.SP;
        protected HackSymbol LCL => HackPlatform.ReservedRegisters.LCL;
        protected HackSymbol ARG => HackPlatform.ReservedRegisters.ARG;
        protected HackSymbol THIS => HackPlatform.PointerRegisters[0];
        protected HackSymbol THAT => HackPlatform.PointerRegisters[1];
        protected HackSymbol R13 => HackPlatform.GeneralRegisters.R13;
        protected HackSymbol R14 => HackPlatform.GeneralRegisters.R14;
        protected HackSymbol R15 => HackPlatform.GeneralRegisters.R15;
    }
}

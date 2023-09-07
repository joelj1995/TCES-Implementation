using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal static class HackPlatform
    {
        #region AssemblerSymbols
        public static readonly string?[] Dests =
        {
            null,
            "M",
            "D",
            "MD",
            "A",
            "AM",
            "AD",
            "AMD"
        };

        public static readonly string[] Comps =
        {
            "0",
            "1",
            "-1",
            "D",
            "A",
            "!D",
            "!A",
            "-D",
            "-A",
            "D+1",
            "A+1",
            "D-1",
            "A-1",
            "D+A",
            "A+D", // Not in Fig 4.3 but it seems to work
            "D-A",
            "A-D",
            "D&A",
            "A&D", // Also not in spec but works
            "D|A",
            "A|D", // Also not in spec but works
            "M",
            "!M",
            "-M",
            "M+1",
            "M-1",
            "D+M",
            "M+D", // Also not in spec but works
            "D-M",
            "M-D",
            "D&M",
            "D|M"
        };

        public static readonly string?[] Jumps =
        {
            null,
            "JGT",
            "JEQ",
            "JGE",
            "JLT",
            "JNE",
            "JLE",
            "JMP"
        };
        #endregion

        #region Registers
        public static class ReservedRegisters
        {
            public static HackSymbol SP => new HackSymbol("SP");
            public static HackSymbol LCL => new HackSymbol("LCL");
            public static HackSymbol ARG => new HackSymbol("ARG");
        }

        public static readonly HackSymbol[] PointerRegisters = new HackSymbol[2]
        {
            new HackSymbol("THIS"),
            new HackSymbol("THAT")
        };

        public static readonly HackSymbol[] TemporaryRegisters = new HackSymbol[8]
        {
            new HackSymbol("R5"),
            new HackSymbol("R6"),
            new HackSymbol("R7"),
            new HackSymbol("R8"),
            new HackSymbol("R9"),
            new HackSymbol("R10"),
            new HackSymbol("R11"),
            new HackSymbol("R12")
        };

        public static class GeneralRegisters
        {
            public static HackSymbol R13 => new HackSymbol("R13");
            public static HackSymbol R14 => new HackSymbol("R14");
            public static HackSymbol R15 => new HackSymbol("R15");
        }
        #endregion
        public static class MemorySegments
        {
            // Page 141
            // Note that the end index of a range is exclusive (hence the +1)
            public static Range Registers => new Range(0, 15 + 1);
            public static Range StaticVariables => new Range(16, 255);
            public static Range Stack => new Range(256, 2047 + 1);
            public static Range Heap => new Range(2048, 16383 + 1);
            public static Range IO => new Range(16384, 24575 + 1);
        }
    }
}

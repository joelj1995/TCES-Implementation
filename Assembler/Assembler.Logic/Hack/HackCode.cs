using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Logic.Hack
{
    public static class HackCode
    {
        public static readonly Dictionary<string, string> DestTranslation = new Dictionary<string, string>()
        {
            {"M", "001"},
            {"D", "010"},
            {"MD", "011"},
            {"A", "100"},
            {"AM", "101"},
            {"AD", "110"},
            {"AMD", "111"},
        };

        public static readonly Dictionary<string, string> CompTranslation = new Dictionary<string, string>()
        {
            { "0", "0" + "101010" },
            { "1", "0" + "111111" },
            { "-1", "0" + "111010" },
            { "D", "0" + "001100" },
            { "A", "0" + "110000" },
            { "!D", "0" + "001101" },
            { "!A", "0" + "110001" },
            { "-D", "0" + "001111" },
            { "-A", "0" + "110011" },
            { "D+1", "0" + "011111" },
            { "A+1", "0" + "110111" },
            { "D-1", "0" + "001110" },
            { "A-1", "0" + "110010" },
            { "D+A", "0" + "000010" },
            { "D-A", "0" + "010011" },
            { "A-D", "0" + "000111" },
            { "D&A", "0" + "000000" },
            { "D|A", "0" + "010101" },
            { "M", "1" + "110000" },
            { "!M", "1" + "110001" },
            { "-M", "1" + "110011" },
            { "M+1", "1" + "110111" },
            { "M-1", "1" + "110010" },
            { "D+M", "1" + "000010" },
            { "D-M", "1" + "010011" },
            { "M-D", "1" + "000111" },
            { "D&M", "1" + "000000" },
            { "D|M", "1" + "010101" },
        };

        public static readonly Dictionary<string, string> JumpTranslation = new Dictionary<string, string>()
        {
            { "JGT", "001" },
            { "JEQ", "010" },
            { "JGE", "011" },
            { "JLT", "100" },
            { "JNE", "101" },
            { "JLE", "110" },
            { "JMP", "111" }
        };


        public static string dest(string? mnemonic)
        {
            if (mnemonic == null)
                return "000";
            return DestTranslation[mnemonic];
        }

        public static string comp(string mnemonic)
        {
            return CompTranslation[mnemonic];
        }

        public static string jump(string? mnemonic)
        {
            if (mnemonic == null)
                return "000";
            return JumpTranslation[mnemonic];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Logic.Hack
{
    public class HackAssembler : IAssembler
    {
        public HackSymbolTable Symbols { get; set; } = new HackSymbolTable();

        public byte[] assemble(StreamReader assembly)
        {
            try
            {
                Symbols = new HackSymbolTable();
                BuildSymbolTable(assembly);
                assembly.BaseStream.Seek(0, SeekOrigin.Begin);
                var binaryCode = GenerateBinaryCode(assembly);
                return Encoding.ASCII.GetBytes(binaryCode);
            }
            catch(ParserException e)
            {
                if (e.lineNumber != null)
                    throw new SyntaxException((int) e.lineNumber, e.reason);
                throw e;
            }
        }

        public void BuildSymbolTable(StreamReader assembly)
        {
            var parser = new HackParser(assembly);
            while (parser.hasMoreCommands())
            {
                parser.advance();
                if (parser.commandType() == CommandType.L_COMMAND)
                    Symbols.addEntry(parser.symbol(), parser.LogicalPosition);
            }
        }

        public string GenerateBinaryCode(StreamReader assembly)
        {
            string result = String.Empty;
            var parser = new HackParser(assembly);
            while (parser.hasMoreCommands())
            {
                parser.advance();
                if (parser.commandType() == CommandType.EMPTY || parser.commandType() == CommandType.L_COMMAND) continue;
                result += BinaryForParserPosition(parser) + "\r\n";
            }
            return result;
        }

        public string BinaryForParserPosition(HackParser parser)
        {
            switch (parser.commandType())
            {
                case CommandType.A_COMMAND:
                    int symbolAsInt = -1;
                    if (int.TryParse(parser.symbol(), out symbolAsInt)) { }
                    else
                        symbolAsInt = GetOrAddMemorySymbol(parser.symbol());
                    return A_CODE + BinaryTool.ExpandIntegerBinary(symbolAsInt, 15);
                case CommandType.C_COMMAND:
                    return (parser.userSpace() ? C_CODE : C_CODE_K) + HackCode.comp(parser.comp()) + HackCode.dest(parser.dest()) + HackCode.jump(parser.jump());
                default:
                    throw new ArgumentException();
            }
        }

        private int GetOrAddMemorySymbol(string symbol)
        {
            if (!Symbols.contains(symbol))
                Symbols.addEntry(symbol, memoryUsedBytes++);
            return Symbols.getAddress(symbol);
        }

        private int memoryUsedBytes = 16;

        private static string A_CODE = "0";
        private static string C_CODE = "111";
        private static string C_CODE_K = "110";
    }
}

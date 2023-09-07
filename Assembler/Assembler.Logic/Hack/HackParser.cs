using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler.Logic.Hack
{
    public enum CommandType
    {
        A_COMMAND,
        C_COMMAND,
        L_COMMAND,
        EMPTY
    }

    public class HackParser
    {
        public int LineNumber { get => lineNumber; }
        public int LogicalPosition { get => logicalPosition; }

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
            "A+D",
            "D-A",
            "A-D",
            "D&A",
            "D|A",
            "M",
            "!M",
            "-M",
            "M+1",
            "M-1",
            "D+M",
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

        public HackParser(StreamReader inputStream) 
        { 
            this.inputStream = inputStream;
        }

        public bool hasMoreCommands()
        {
            return !inputStream.EndOfStream;
        }

        public void advance()
        {
            currentCommand = inputStream.ReadLine();
            lineNumber++;
            if (commandType() == CommandType.C_COMMAND || commandType() == CommandType.A_COMMAND)
                logicalPosition++;
        }

        public CommandType commandType()
        {
            var command = cleanCommand();
            if (command.Equals(String.Empty) || command.StartsWith("//"))
            {
                return CommandType.EMPTY;
            }
            if (command.StartsWith("(") && command.EndsWith(")")) 
            {
                return CommandType.L_COMMAND;
            }
            if (command.StartsWith("@"))
            {
                return CommandType.A_COMMAND;
            }
            return CommandType.C_COMMAND;
        }

        public string symbol()
        {
            if (commandType() != CommandType.L_COMMAND && commandType() != CommandType.A_COMMAND)
                throw new InvalidOperationException("Invalid command type.");
            var command = cleanCommand();
            if (commandType() == CommandType.L_COMMAND)
                return command.Substring(1, command.Length - 2);
            else
                return command.Substring(1, command.Length - 1);
        }

        public string? dest()
        {
            if (commandType() != CommandType.C_COMMAND)
                throw new InvalidOperationException("Invalid command type.");
            var command = cleanCommand();
            if (!command.Contains("="))
                return null;
            var result = command.Split("=")[0];
            if (!Dests.Contains(result))
                throw new ParserException($"{result} is not a recognzied dest.", lineNumber);
            return result;
        }

        public string comp()
        {
            if (commandType() != CommandType.C_COMMAND)
                throw new InvalidOperationException("Invalid command type.");
            var command = cleanCommand();
            var idxStart = 0;
            var idxEnd = command.Length;
            if (command.Contains("="))
            {
                idxStart = command.IndexOf("=") + 1;
            }
            if (command.Contains(";"))
            {
                idxEnd = command.IndexOf(";");
            }
            else if (command.EndsWith("!"))
            {
                idxEnd = command.Length - 1;
            }
            var result = command.Substring(idxStart, idxEnd - idxStart);
            if (!Comps.Contains(result))
                throw new ParserException($"{result} is not a recognzied comp.", lineNumber);
            return result;
        }

        public string? jump()
        {
            if (commandType() != CommandType.C_COMMAND)
                throw new InvalidOperationException("Invalid command type.");
            var command = cleanCommand();
            var jumpIdx = command.IndexOf(";");
            if (jumpIdx == -1)
                return null;
            var result = command.Substring(jumpIdx + 1);
            if (!Jumps.Contains(result))
                throw new ParserException($"{result} is not a recognzied dest.", lineNumber);
            return result;
        }

        public bool userSpace()
        {
            if (commandType() != CommandType.C_COMMAND)
                throw new InvalidOperationException("Invalid command type.");
            var command = cleanCommand();
            return !command.EndsWith("!");
        }

        private string cleanCommand()
        {
            if (currentCommand == null)
                throw new InvalidOperationException("Parser has not advanced to a command.");
            var effectiveLength = currentCommand.IndexOf("//") == -1 ? currentCommand.Length : currentCommand.IndexOf("//");
            return currentCommand.Substring(0, effectiveLength).Trim();
        }

        private StreamReader inputStream;
        private string? currentCommand = null;
        private int lineNumber = 0;
        private int logicalPosition = 0;
    }
}

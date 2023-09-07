using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMtranslator.Core.Interface;

namespace VMtranslator.Core
{
    internal class VMParser : IParser
    {
        public VMParser(StreamReader inputStream)
        {
            this.inputStream = inputStream;
        }

        public void advance()
        {
            if (!hasMoreCommands())
                throw new InvalidOperationException("No more commands to read.");
            currentLine = inputStream.ReadLine();
            lineNumber++;
        }

        public string arg1()
        {
            var line = cleanLine();
            var command = line.Split(" ")[0];
            if (commandType() == CommandType.C_ARITHMETIC)
                return command;
            return lineParts()[1];
        }

        public int arg2()
        {
            var commandType = this.commandType();
            if (commandType != CommandType.C_PUSH &&
                commandType != CommandType.C_POP &&
                commandType != CommandType.C_FUNCTION &&
                commandType != CommandType.C_CALL)
                throw new InvalidOperationException($"arg 2 cannot be called for command type {commandType}");
            int result = -1;
            if (!int.TryParse(lineParts()[2], out result))
                throw new SyntaxException(lineNumber, "Expected an integer, got: {this.lineParts()[2]}");
            return result;
        }

        public CommandType commandType()
        {
            var line = cleanLine();
            if (line.Equals(string.Empty) || line.StartsWith("//"))
                return CommandType.EMPTY;
            var command = lineParts()[0];
            if (!VMConstants.commandTypeMappings.ContainsKey(command))
                throw new SyntaxException(lineNumber, $"Command not recognize: {command}");
            return VMConstants.commandTypeMappings[command];
        }

        public bool hasMoreCommands()
        {
            return !inputStream.EndOfStream;
        }

        private string cleanLine()
        {
            if (currentLine == null)
                throw new InvalidOperationException("Parser has not advanced to a command.");
            var effectiveLength = currentLine.IndexOf("//") == -1 ? currentLine.Length : currentLine.IndexOf("//");
            return currentLine.Substring(0, effectiveLength).Trim();
        }

        private string[] lineParts()
        {
            var line = cleanLine();
            return line.Split(" ");
        }

        private StreamReader inputStream;
        private string? currentLine = null;
        private int lineNumber = 0;
    }
}

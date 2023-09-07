using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal class HackScript
    {
        public readonly IHackCommand[] commands;

        public static HackScript create()
        {
            return new HackScript(Array.Empty<IHackCommand>());
        }

        public static HackScript create(IHackCommand hackCommand)
        {
            return new HackScript(hackCommand);
        }

        public static HackScript create(IEnumerable<IHackCommand> commands)
        {
            return new HackScript(commands.ToArray());
        }

        private HackScript(IHackCommand hackCommand)
        {
            commands = new IHackCommand[] { hackCommand };
        }

        private HackScript(IHackCommand[] commands)
        {
            this.commands = commands;
        }

        public HackScript clone()
        {
            return new HackScript(this.commands);
        }

        public static HackScript operator+ (HackScript scriptA, HackScript scriptB)
        {
            return new HackScript(scriptA.commands.Concat(scriptB.commands).ToArray());
        }

        public static HackScript operator +(IHackCommand commandA, HackScript scriptB)
        {
            var commands = new IHackCommand[] { commandA }.Concat(scriptB.commands);
            return new HackScript(commands.ToArray());
        }

        public static HackScript operator +(HackScript scriptA, IHackCommand commandB)
        {
            var commands = scriptA.commands.Concat(new IHackCommand[] { commandB });
            return new HackScript(commands.ToArray());
        }

        public override string ToString() 
        {
            var sb = new StringBuilder();
            foreach (var command in commands) sb.AppendLine(command.encode());
            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal abstract class HackCommand
    {
        public static IHackCommand A(HackSymbol symbol)
        {
            return new HackCommandA(symbol);
        }

        public static IHackCommand A(string symbol)
        {
            return new HackCommandA(new HackSymbol(symbol));
        }

        public static IHackCommand A(int address)
        {
            return new HackCommandA(new HackSymbol(address));
        }

        public static IHackCommand C(string? dest, string comp, string? jump)
        {
            return new HackCommandC(dest, comp, jump);
        }

        public static IHackCommand C(string dest, string comp)
        {
            return new HackCommandC(dest, comp, null);
        }

        public static IHackCommand L(HackSymbol symbol)
        {
            return new HackCommandL(symbol);
        }

        public static IHackCommand L(string symbol)
        {
            return new HackCommandL(symbol);
        }

        public static IHackCommand Comment(string comment)
        {
            return new HackComment(comment);
        }

        public static IHackCommand CommentF(string comment) 
        {
            return Comment($" {comment}");
        }
    }
}

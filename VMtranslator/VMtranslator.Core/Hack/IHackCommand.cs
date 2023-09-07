using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal interface IHackCommand
    {
        IHackCommand clone();
        string encode();

        static HackScript operator +(IHackCommand a, IHackCommand b)
        {
            return HackScript.create(a) + HackScript.create(b);
        }

        HackScript toScript()
        {
            return HackScript.create(this);
        }
    }
}

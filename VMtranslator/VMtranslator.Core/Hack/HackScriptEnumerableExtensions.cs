using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMtranslator.Core.Hack
{
    internal static class HackScriptEnumerableExtensions
    {
        public static HackScript Sum(this IEnumerable<HackScript> container)
        {
            if (container.Count() == 0)
                return HackScript.create();
            return container.Aggregate((script, next) => script + next);
        }
    }
}

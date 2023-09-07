using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTest
{
    internal class ArrayEndToEndTest : AbstractEndToEndTest
    {
        public override EndToEndTestConfiguration MyConfig => new EndToEndTestConfiguration("ArrayTest", imports);
        private static readonly ICollection<string> imports = new[] { "Array", "Memory", "Sys" };
    }
}

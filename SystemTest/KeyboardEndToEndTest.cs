using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTest
{
    internal class KeyboardEndToEndTest : AbstractEndToEndTest
    {
        public override EndToEndTestConfiguration MyConfig => new EndToEndTestConfiguration("KeyboardTest", OSImports);
    }
}

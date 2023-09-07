using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTest
{
    public class EndToEndTestConfiguration
    {
        public EndToEndTestConfiguration(string TestName) 
        {
            this.TestName = TestName;
        }

        public string TestName { get; private set; }
    }
}

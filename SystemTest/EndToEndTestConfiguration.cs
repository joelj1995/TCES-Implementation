using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTest
{
    public class EndToEndTestConfiguration
    {
        public EndToEndTestConfiguration(string testName, ICollection<string> imports) 
        {
            this.TestName = testName;
            this.LibImports = imports;
        }

        public string TestName { get; private set; }
        public ICollection<string> LibImports { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTest
{
    internal static class TestConfig
    {
        public static string TECSSoftwareRoot => Environment.GetEnvironmentVariable("TECS_SuiteRoot") ?? throw new ArgumentException("TES software location not defined");
        public static string RepositoryRoot => Environment.GetEnvironmentVariable("TECS_RepoRoot") ?? "..\\..\\..\\..\\";
    }
}

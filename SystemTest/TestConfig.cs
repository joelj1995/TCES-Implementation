using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTest
{
    internal static class TestConfig
    {
        public static string RepositoryRoot => Environment.GetEnvironmentVariable("TECS_RepoRoot") ?? "..\\..\\..\\..\\";
    }
}

using System.Diagnostics;

namespace SystemTest
{
    public abstract class AbstractEndToEndTest
    {
        public abstract string TestName { get; }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEndToEnd()
        {
            Compile();
            Translate();
            Assemble();
            Execute();
        }

        private void Compile()
        {
            var command = Path.Join(TestConfig.RepositoryRoot, "JackCompiler\\HackCompiler.CLI\\bin\\Release\\net6.0\\JackCompiler.CLI.exe");
            var parameter = Path.Join(TestConfig.RepositoryRoot, $"SystemTest\\Samples\\{TestName}");
            var fullScript = $"& '{command}' '{parameter}'";
            ExecutePowershellScript(fullScript);
        }

        private void Translate()
        {

        }

        private void Assemble()
        {

        }

        private void Execute()
        {

        }
        
        private void ExecutePowershellScript(string script)
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "powershell.exe";
            processStartInfo.Arguments = $"-Command \"{script}\"";
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
        }
    }
}
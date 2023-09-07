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
            CompileLib();
            StageFiles();
            Compile();
            Translate();
            Assemble();
            Execute();
        }

        private void CompileLib()
        {
            var command = Path.Join(TestConfig.RepositoryRoot, "JackCompiler\\HackCompiler.CLI\\bin\\Release\\net6.0\\JackCompiler.CLI.exe");
            var parameter = Path.Join(TestConfig.RepositoryRoot, $"Lib");
            var fullScript = $"& '{command}' '{parameter}'";
            ExecutePowershellScript(fullScript);
        }

        private void StageFiles()
        {
            var command = "Copy-Item";
            var parameter1 = Path.Join(TestConfig.RepositoryRoot, $"Lib\\*.vm");
            var parameter2 = Path.Join(TestConfig.RepositoryRoot, $"SystemTest\\Staging\\{TestName}");
            var fullScript = $"{command} '{parameter1}' '{parameter2}'";
            ExecutePowershellScript(fullScript);
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
            processStartInfo.RedirectStandardError = true;

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var err = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (process.ExitCode != 0) throw new Exception($"Out:\n{output}\nErr:\n{err}");
        }
    }
}
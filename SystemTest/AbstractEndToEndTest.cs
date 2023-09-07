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
            Clean();
            StageFiles();
            Compile();
            Translate();
            Assemble();
            Execute();
        }

        private void Clean()
        {
            ExecutePowershellScript($"Remove-Item -Force -Recurse '{StagingDir}'; Write-Host '';");
            ExecutePowershellScript($"New-Item -Type Directory '{StagingDir}'");
        }

        private void StageFiles()
        {
            var command = "Copy-Item";
            var parameter1 = Path.Join(TestConfig.RepositoryRoot, $"Lib\\*.jack");
            var parameter2 = Path.Join(TestConfig.RepositoryRoot, $"SystemTest\\Staging\\");
            var fullScript = $"{command} '{parameter1}' '{parameter2}'";
            ExecutePowershellScript(fullScript);
            parameter1 = Path.Join(TestConfig.RepositoryRoot, $"SystemTest\\Samples\\{TestName}\\*.jack");
            fullScript = $"{command} '{parameter1}' '{parameter2}'";
            ExecutePowershellScript(fullScript);
        }

        private void Compile()
        {
            var command = Path.Join(TestConfig.RepositoryRoot, "JackCompiler\\HackCompiler.CLI\\bin\\Release\\net6.0\\JackCompiler.CLI.exe");
            var parameter = StagingDir;
            var fullScript = $"& '{command}' '{parameter}\\'";
            ExecutePowershellScript(fullScript);
        }

        private void Translate()
        {
            var command = Path.Join(TestConfig.RepositoryRoot, "VMTranslator\\VMTranslator.CLI\\bin\\Release\\net6.0\\VMTranslator.CLI.exe");
            var parameter = StagingDir;
            var fullScript = $"& '{command}' '{parameter}\\'";
            ExecutePowershellScript(fullScript);
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

        public string StagingDir => Path.Join(TestConfig.RepositoryRoot, "\\SystemTest\\Staging\\");
    }
}
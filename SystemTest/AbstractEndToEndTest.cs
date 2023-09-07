using System.Diagnostics;

namespace SystemTest
{
    public abstract class AbstractEndToEndTest
    {
        public abstract EndToEndTestConfiguration MyConfig { get; }

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
            var paramStagingDir = Path.Join(TestConfig.RepositoryRoot, $"SystemTest\\Staging\\");
            foreach (var import in MyConfig.LibImports)
            {
                var paramLib = Path.Join(TestConfig.RepositoryRoot, "Lib\\", $"{import}.jack");
                var copyLibScript = $"{command} '{paramLib}' '{paramStagingDir}'";
                ExecutePowershellScript(copyLibScript);
            }
            var paramJackFiles = Path.Join(TestConfig.RepositoryRoot, $"SystemTest\\Samples\\{MyConfig.TestName}\\*");
            var fullScript = $"{command} '{paramJackFiles}' '{paramStagingDir}'";
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
            var command = Path.Join(TestConfig.RepositoryRoot, "Assembler\\Assembler.CLI\\bin\\Release\\net6.0\\Assembler.CLI.exe");
            var parameter = StagingDir;
            var fullScript = $"& '{command}' '{parameter}\\'";
            ExecutePowershellScript(fullScript);
        }

        private void Execute()
        {
            var command = Path.Join(TestConfig.TECSSoftwareRoot, "CPUEmulator.bat");
            var parameter = Path.GetFullPath(Path.Join(StagingDir, $"{MyConfig.TestName}.tst"));
            var fullScript = $"& '{command}' '{parameter}'";
            ExecutePowershellScript(fullScript);
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

        public string StagingDir => Path.Join(TestConfig.RepositoryRoot, "SystemTest\\Staging\\");
    }
}
using JackCompiler.Lib.Jack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler.Lib
{
    public class JackCompiler : ICompiler
    {
        public void Compile(string path)
        {
            if (Directory.Exists(path)) 
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    if (file.EndsWith(".jack")) Compile(file); 
                }
            }
            else if (File.Exists(path))
            {
                CompileFile(path);
            }
            else
            {
                throw new ArgumentException($"Path {path} does not exist");
            }
        }

        private void CompileFile(string path)
        {
            if (!path.EndsWith(".jack"))
                throw new AggregateException($"{path} is not a jack file.");
            var outputPath = path.Replace(".jack", ".vm");
            using (var fileReader = File.OpenRead(path))
            using (var fileWriter = File.Create(outputPath))
            using (var inputStream = new StreamReader(fileReader))
            using (var outputStream = new StreamWriter(fileWriter))
            {
                IProcessor processor = new JackProcessor(config, inputStream, outputStream);
                processor.Process();
            }
        }

        private readonly JackConfiguration config = new JackConfiguration();
    }
}

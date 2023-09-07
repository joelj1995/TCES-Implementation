using JackCompiler.Lib.Jack;

namespace JackCompiler.Lib
{
    public static class ProcessorFactory
    {
        public static IProcessor CreateForXmlOutput(StreamReader inputStream, StreamWriter outputStream)
        {
            var config = new JackConfiguration()
            {
                OutputXml = true
            };
            return new JackProcessor(config, inputStream, outputStream);
        }
    }
}
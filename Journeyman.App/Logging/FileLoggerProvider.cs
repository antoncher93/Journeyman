using Microsoft.Extensions.Logging;

namespace Journeyman.App.Logging
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _path;
        public FileLoggerProvider(string path)
        {
            _path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new TextFileLogger(_path);
        }

        public void Dispose()
        {
        }
    }
}

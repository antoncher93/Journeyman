using Microsoft.Extensions.Logging;

namespace Journeyman.App.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddFile(this ILoggingBuilder loggingBuilder, string fileName)
        {
            loggingBuilder.AddProvider(new FileLoggerProvider(fileName));
            return loggingBuilder;
        }
    }
}

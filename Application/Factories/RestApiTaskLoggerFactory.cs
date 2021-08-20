using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Serilog;

namespace Application.Factories
{
    public static class RestApiTaskLoggerFactory
    {
        public static ILoggerFactory Create(IConfiguration configuration)
        {
            var loggingConfig = configuration.GetSection("Logging");
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConfiguration(loggingConfig);
                builder.ClearProviders();
                builder.AddConsole(options =>
                {
                    options.FormatterName = ConsoleFormatterNames.Systemd;
                    options.LogToStandardErrorThreshold = LogLevel.Information;
                });
                builder.AddDebug();
            });
            var fileName = Assembly.GetCallingAssembly().GetName().Name?.Replace('.', '_');

            var loggingDir = CanCreateLogFiles(configuration);

            var filePath = Path.Combine(loggingDir, $"{fileName}.txt");

            var log = new LoggerConfiguration()
                .WriteTo.RollingFile(filePath,
                    retainedFileCountLimit: 10,
                    outputTemplate:
                    "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {Level:u3}][{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            loggerFactory.AddSerilog(log, true);
            return loggerFactory;
        }

        private static string CanCreateLogFiles(IConfiguration configuration)
        {
            var loggingDir = configuration.GetSection("Logging:Directory")?.Value;
            if (string.IsNullOrEmpty(loggingDir))
            {
                throw new Exception("Logging directory path must be set in appsetings.json");
            }

            if (!Directory.Exists(loggingDir))
            {
                Directory.CreateDirectory(loggingDir);
            }
            
            return loggingDir;
        }
    }
}
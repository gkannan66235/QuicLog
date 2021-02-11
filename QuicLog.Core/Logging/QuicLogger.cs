using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;



namespace QuicLog.Core.Logging
{
    public static class QuicLogger
    {
        private static IConfiguration _configuration;
        private static QuicLogConfigOptions _options;

        public static void Configure( string configSection = "QuicLog")
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    optional: false)
                .Build();
            _options = QuicLogConfigOptions.Create(_configuration, configSection);
            CreateLogger();
        }
        public static void LogAndFlush()
        {
            Log.CloseAndFlush();
        }
        public static void Info(string message)
        {
            Log.Logger.Information(message);
            LogAndFlush();
        }
        private static void CreateLogger()
        {
            Log.Logger = CreateLoggerFromConfiguration();
        }

        private static ILogger CreateLoggerFromConfiguration()
        {
            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration);
            if (_options.IsEventCollector)
                return CreateEventCollectorLogger(loggerConfig);
            if (_options.IsConsole)
                return CreateConsoleLogger(loggerConfig);
            return CreateConsoleLogger(loggerConfig);
        }

        private static ILogger CreateConsoleLogger(LoggerConfiguration loggerConfig)
        {
            return loggerConfig.WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}").CreateLogger();
        }   

        private static ILogger CreateEventCollectorLogger(LoggerConfiguration loggerConfig)
        {
            return loggerConfig.WriteTo.EventCollector("http://localhost:8088/services/collector", "e8f47278-7ff0-4a33-a370-672afa4879b8").CreateLogger();
        }
    }
}
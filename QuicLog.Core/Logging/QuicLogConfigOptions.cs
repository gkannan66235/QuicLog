using System;
using Microsoft.Extensions.Configuration;

namespace QuicLog.Core.Logging
{
    public class QuicLogConfigOptions
    {
        private QuicLogConfigOptions( bool isConsole,  bool isRollingFile,  bool isApplicationInsights)
        {
            IsConsole = isConsole;
            IsRollingFile = isRollingFile;
            IsApplicationInsights = isApplicationInsights;
        }

        public bool IsRollingFile { get; private set; }
        public bool IsApplicationInsights { get; private set; }
        public bool IsConsole { get; private set; }
        public string InstrumentationKey { get; private set; }
        public string RollingFilePath { get; private set; }

        public static QuicLogConfigOptions Create(IConfiguration configuration, string configSection)
        {
            QuicLogConfigOptions options;
            var quicLogSection = configuration.GetSection(configSection);
            if (quicLogSection == null)
                throw new InvalidOperationException("Section QuicLog is missing.");
            if (bool.TryParse(quicLogSection["IsConsole"], out var isConsole) &&
                bool.TryParse(quicLogSection["IsRollingFile"], out var isRollingFile) &&
                bool.TryParse(quicLogSection["IsApplicationInsights"], out var isApplicationInsights))
            {
                options = new QuicLogConfigOptions(isConsole, isRollingFile, isApplicationInsights);
            }
            else
                throw new InvalidOperationException("Unable to parse boolean values in QuicLog section");

            if (isApplicationInsights)
            {
                var instrumentationKey = configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
                if (string.IsNullOrWhiteSpace(instrumentationKey))
                {
                    throw new InvalidOperationException(
                        "Instrumentation key cannot be null. It should be set under ApplicationInsights:InstrumentationKey");
                }

                options.InstrumentationKey = instrumentationKey;
            }

            if (isRollingFile)
            {
                options.RollingFilePath = quicLogSection["RollingFilePath"];
            }
            return options;
        }
    }
}
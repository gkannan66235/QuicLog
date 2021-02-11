using System;
using Microsoft.Extensions.Configuration;

namespace QuicLog.Core.Logging
{
    public class QuicLogConfigOptions
    {
        private QuicLogConfigOptions( bool isConsole,  bool isEventCollector)
        {
            IsConsole = isConsole;
            IsEventCollector = isEventCollector;
        }

        public bool IsEventCollector { get; private set; }
        public bool IsConsole { get; private set; }
        public string SplunkEndpoint { get; private set; }
        public string SplunkToken { get; private set; }

        public static QuicLogConfigOptions Create(IConfiguration configuration, string configSection)
        {
            QuicLogConfigOptions options;
            var quicLogSection = configuration.GetSection(configSection);
            if (quicLogSection == null)
                throw new InvalidOperationException("Section QuicLog is missing.");
            if (bool.TryParse(quicLogSection["IsConsole"], out var isConsole) &&
                bool.TryParse(quicLogSection["IsEventCollector"], out var isEventCollector))
            {
                options = new QuicLogConfigOptions(isConsole, isEventCollector);
            }
            else
                throw new InvalidOperationException("Unable to parse boolean values in QuicLog section");

            if (isEventCollector)
            {
                var SplunkToken = configuration.GetValue<string>("EventCollector:SplunkToken");
                var SplunkEndpoint = configuration.GetValue<string>("EventCollector:SplunkEndpoint");
                if (string.IsNullOrWhiteSpace(SplunkToken))
                {
                    throw new InvalidOperationException(
                        "Splunk Token cannot be null. It should be set under EventCollector:SplunkToken");
                }
                if (string.IsNullOrWhiteSpace(SplunkEndpoint))
                {
                    throw new InvalidOperationException(
                        "Splunk Endpoint cannot be null. It should be set under EventCollector:SplunkEndpoint");
                }
                options.SplunkToken = SplunkToken;
                options.SplunkEndpoint = SplunkEndpoint;
            }
            return options;
        }
    }
}
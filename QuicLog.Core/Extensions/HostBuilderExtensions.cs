using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace QuicLog.Core.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseQuicLog(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog();
            return hostBuilder;
        }
        public static Void Test() {}
    }
}
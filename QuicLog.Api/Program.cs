using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using QuicLog.Core.Extensions;
using QuicLog.Core.Logging;

namespace QuicLog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            QuicLogger.Configure();
            try
            {
                QuicLogger.Info("Api Started successfully");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                QuicLogger.Info("Api stopper and thank you");
                QuicLogger.LogAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseQuicLog()
        ;
    }
}
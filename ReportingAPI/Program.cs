using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReportingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                }).ConfigureAppConfiguration(appConfig =>
                {
                    appConfig.AddJsonFile($"appsettings.json", false, true);
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Length < 3)
                        throw new Exception("Missing Environment Variable(ASPNETCORE_ENVIRONMENT)");
                    appConfig.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", false, true);
                });
    }
}

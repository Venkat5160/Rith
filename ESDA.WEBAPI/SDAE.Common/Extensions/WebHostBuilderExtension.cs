using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SDAE.Common.Extensions
{
    public static class WebHostBuilderExtension
    {
        public static IWebHostBuilder SetHostProps(this IWebHostBuilder webHost, string[] args)
        {
            return webHost
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                    if (string.IsNullOrEmpty(envName))
                        envName = "Production";

                    string appStartupPath = AppDomain.CurrentDomain.BaseDirectory;

                    // Prepare configuration builder
                    config.SetBasePath(appStartupPath)
                            //.AddJsonFile("globalsettings.json", false, true)
                            //.AddJsonFile($"globalsettings.{envName}.json", false, true)
                            .AddJsonFile("appsettings.json", false, true)
                            //.AddJsonFile($"appsettings.{envName}.json", false, true)
                            .AddEnvironmentVariables();
                });
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Playprism.ApiGateway
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
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        config.AddJsonFile("ocelot.development.json");
                    }
                    else if (hostingContext.HostingEnvironment.IsEnvironment("Docker"))
                    {
                        Console.WriteLine("DEBUG: got ocelot.docker.json file");
                        config.AddJsonFile("ocelot.docker.json");
                    }
                    else if (hostingContext.HostingEnvironment.IsProduction())
                    {
                        Console.WriteLine("DEBUG: got ocelot.production.json file");
                        config.AddJsonFile("ocelot.production.json");
                    }
                    else
                    {
                        config.AddJsonFile("ocelot.json");
                    }
                });
    }
}

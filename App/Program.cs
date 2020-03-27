using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App
{
    public static class Program
    {
        public static void Main()
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();

            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }
    }
}

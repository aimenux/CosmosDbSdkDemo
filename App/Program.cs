using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibSdk2;
using LibSdk2.Models.CreateModels;
using LibSdk2.Models.DestroyModels;
using LibSdk2.Models.InsertModels;
using LibSdk2.Models.QueryModels;
using LibSdk2.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static Bullseye.Targets;

namespace App
{
    public static class Program
    {
        public const string DefaultEnvironmentToUse = "DEV";

        public static async Task Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT") ?? DefaultEnvironmentToUse;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.Configure<CosmosDbQueries>(configuration.GetSection(nameof(CosmosDbQueries)));
            services.Configure<CosmosDbSettings>(configuration.GetSection(nameof(CosmosDbSettings)));
            services.Configure<CosmosDbDocuments>(configuration.GetSection(nameof(CosmosDbDocuments)));
            services.AddSingleton<ICosmosDbPrinter, CosmosDbPrinter>();
            services.AddSingleton<ICosmosDbRepository>(provider =>
            {
                var cosmosDbSettings = provider.GetService<IOptions<CosmosDbSettings>>().Value;
                return new CosmosDbRepository(cosmosDbSettings);
            });

            var serviceProvider = services.BuildServiceProvider();

            Target(nameof(Targets.Default), DependsOn(nameof(Targets.Create), nameof(Targets.Destroy)));

            Target(nameof(Targets.Create), async () =>
            {
                WaitForTargetConfirmation(nameof(Targets.Create));
                var printer = serviceProvider.GetRequiredService<ICosmosDbPrinter>();
                var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
                var cosmosDbSettings = serviceProvider.GetService<IOptions<CosmosDbSettings>>().Value;
                var request = new CosmosDbCreateRequest(cosmosDbSettings);
                var response = await repository.CreateCosmosDbAsync(request);
                printer.Print(request, response);
            });

            Target(nameof(Targets.Insert), DependsOn(nameof(Targets.Create)), async () =>
            {
                WaitForTargetConfirmation(nameof(Targets.Insert));
                var printer = serviceProvider.GetRequiredService<ICosmosDbPrinter>();
                var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
                var cosmosDbDocuments = serviceProvider.GetService<IOptions<CosmosDbDocuments>>().Value;
                var cosmosDbRequests = cosmosDbDocuments.Select(x => new CosmosDbInsertRequest(x));
                foreach (var request in cosmosDbRequests)
                {
                    var response = await repository.InsertCosmosDbAsync(request);
                    printer.Print(request, response);
                }
            });

            Target(nameof(Targets.Query), DependsOn(nameof(Targets.Insert)), async () =>
            {
                WaitForTargetConfirmation(nameof(Targets.Query));
                var printer = serviceProvider.GetRequiredService<ICosmosDbPrinter>();
                var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
                var cosmosDbQueries = serviceProvider.GetService<IOptions<CosmosDbQueries>>().Value;
                var cosmosDbRequests = cosmosDbQueries.Select(x => new CosmosDbQueryRequest(x));
                foreach (var request in cosmosDbRequests)
                {
                    var response = await repository.QueryCosmosDbAsync(request);
                    printer.Print(request, response);
                }
            });

            Target(nameof(Targets.Destroy), DependsOn(nameof(Targets.Query)), async () =>
            {
                WaitForTargetConfirmation(nameof(Targets.Destroy));
                var printer = serviceProvider.GetRequiredService<ICosmosDbPrinter>();
                var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
                var cosmosDbSettings = serviceProvider.GetService<IOptions<CosmosDbSettings>>().Value;
                var request = new CosmosDbDestroyRequest(cosmosDbSettings);
                var response = await repository.DestroyCosmosDbAsync(request);
                printer.Print(request, response);
            });

            await RunTargetsWithoutExitingAsync(args);

            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }

        private static void WaitForTargetConfirmation(string target)
        {
            ConsoleColor.Yellow.WriteLine($"Press any key to run '{target}'");
            Console.ReadKey();
        }
    }
}

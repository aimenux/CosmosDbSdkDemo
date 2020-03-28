using System;
using System.IO;
using LibSdk2;
using LibSdk2.Models;
using LibSdk2.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static Bullseye.Targets;

namespace App
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.Configure<CosmosDbQueries>(configuration.GetSection(nameof(CosmosDbQueries)));
            services.Configure<CosmosDbSettings>(configuration.GetSection(nameof(CosmosDbSettings)));
            services.AddSingleton<ICosmosDbRepository>(provider =>
            {
                var cosmosDbSettings = provider.GetService<IOptions<CosmosDbSettings>>().Value;
                return new CosmosDbRepository(cosmosDbSettings);
            });

            var serviceProvider = services.BuildServiceProvider();

            Target(nameof(Targets.Default), DependsOn(nameof(Targets.Query)));
            Target(nameof(Targets.Query), async () =>
            {
                var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
                var cosmosDbQueries = serviceProvider.GetService<IOptions<CosmosDbQueries>>().Value;
                foreach (var cosmosDbQuery in cosmosDbQueries)
                {
                    var query = cosmosDbQuery.Query;
                    var enableCrossPartition = cosmosDbQuery.EnableCrossPartition;
                    var request = new CosmosDbRequest(query, enableCrossPartition);
                    var response = await repository.GetCosmosDbResponseAsync<dynamic>(request);
                    PrintSummary(request, response);
                }
            });

            RunTargetsWithoutExiting(args);

            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }

        private static void PrintSummary(CosmosDbRequest request, CosmosDbResponse<dynamic> response)
        {
            Console.WriteLine($"Query: {request.Query}");
            Console.WriteLine($"EnableCrossPartition: {request.Options.EnableCrossPartitionQuery}");
            Console.WriteLine($"Found: {response.Documents.Count} document(s)");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }

        public enum Targets
        {
            Default,
            Query
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bullseye;
using LibSdk3;
using LibSdk3.Models.CreateModels;
using LibSdk3.Models.DeleteModels;
using LibSdk3.Models.DestroyModels;
using LibSdk3.Models.InsertModels;
using LibSdk3.Models.QueryModels;
using LibSdk3.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace App.Bootstrappers
{
    public class Sdk3Bootstrapper : ISdkBootstrapper
    {
        private const string DefaultEnvironmentToUse = "DEV";

        public string Name => "Azure Cosmos Db SDK 3";

        public async Task LaunchAsync(string[] args)
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

            var targets = new Targets();

            targets.Add(nameof(CosmosDbTargets.Create), async () =>
            {
                CosmosDbTargets.Create.WaitForConfirmation();
                var printer = serviceProvider.GetRequiredService<ICosmosDbPrinter>();
                var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
                var cosmosDbSettings = serviceProvider.GetService<IOptions<CosmosDbSettings>>().Value;
                var request = new CosmosDbCreateRequest(cosmosDbSettings);
                var response = await repository.CreateCosmosDbAsync(request);
                printer.Print(request, response);
            });

            targets.Add(nameof(CosmosDbTargets.Insert), new List<string> {nameof(CosmosDbTargets.Create)}, async () =>
            {
                CosmosDbTargets.Insert.WaitForConfirmation();
                var printer = serviceProvider.GetRequiredService<ICosmosDbPrinter>();
                var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
                var cosmosDbSettings = serviceProvider.GetService<IOptions<CosmosDbSettings>>().Value;
                var cosmosDbDocuments = serviceProvider.GetService<IOptions<CosmosDbDocuments>>().Value;
                var cosmosDbRequests = cosmosDbDocuments.Select(x => new CosmosDbInsertRequest(x, cosmosDbSettings));
                foreach (var request in cosmosDbRequests)
                {
                    var response = await repository.InsertCosmosDbAsync(request);
                    printer.Print(request, response);
                }
            });

            targets.Add(nameof(CosmosDbTargets.Query), new List<string> {nameof(CosmosDbTargets.Insert)}, async () =>
            {
                CosmosDbTargets.Query.WaitForConfirmation();
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

            targets.Add(nameof(CosmosDbTargets.Delete), new List<string> {nameof(CosmosDbTargets.Query)}, async () =>
            {
                CosmosDbTargets.Delete.WaitForConfirmation();
                var printer = serviceProvider.GetRequiredService<ICosmosDbPrinter>();
                var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
                var cosmosDbSettings = serviceProvider.GetService<IOptions<CosmosDbSettings>>().Value;
                var cosmosDbDocuments = serviceProvider.GetService<IOptions<CosmosDbDocuments>>().Value;
                var cosmosDbRequests = cosmosDbDocuments.Select(x => new CosmosDbDeleteRequest(x, cosmosDbSettings));
                foreach (var request in cosmosDbRequests)
                {
                    var response = await repository.DeleteCosmosDbAsync(request);
                    printer.Print(request, response);
                }
            });

            targets.Add(nameof(CosmosDbTargets.Destroy), new List<string> {nameof(CosmosDbTargets.Delete)}, async () =>
            {
                CosmosDbTargets.Destroy.WaitForConfirmation();
                var printer = serviceProvider.GetRequiredService<ICosmosDbPrinter>();
                var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
                var cosmosDbSettings = serviceProvider.GetService<IOptions<CosmosDbSettings>>().Value;
                var request = new CosmosDbDestroyRequest(cosmosDbSettings);
                var response = await repository.DestroyCosmosDbAsync(request);
                printer.Print(request, response);
            });

            targets.Add(nameof(CosmosDbTargets.Default), new List<string> {nameof(CosmosDbTargets.Destroy)});

            await targets.RunWithoutExitingAsync(args);
        }
    }
}

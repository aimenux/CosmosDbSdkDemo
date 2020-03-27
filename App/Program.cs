using System;
using System.IO;
using System.Threading.Tasks;
using LibSdk2;
using LibSdk2.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace App
{
    public static class Program
    {
        public static async Task Main()
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.Configure<CosmosDbSettings>(configuration.GetSection(nameof(CosmosDbSettings)));
            services.AddSingleton<ICosmosDbRepository>(provider =>
            {
                var cosmosDbSettings = provider.GetService<IOptions<CosmosDbSettings>>().Value;
                return new CosmosDbRepository(cosmosDbSettings);
            });

            var serviceProvider = services.BuildServiceProvider();
            var query = $"SELECT VALUE c FROM c WHERE c.PartitionKey = \"PUT-YOUR-PK\"";;
            var repository = serviceProvider.GetRequiredService<ICosmosDbRepository>();
            var documents = await repository.GetDocumentsAsync<dynamic>(query);

            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }
    }
}

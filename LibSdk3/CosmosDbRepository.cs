using System;
using System.Threading.Tasks;
using LibSdk3.Models.CreateModels;
using LibSdk3.Models.DeleteModels;
using LibSdk3.Models.DestroyModels;
using LibSdk3.Models.InsertModels;
using LibSdk3.Models.QueryModels;
using LibSdk3.Settings;
using Microsoft.Azure.Cosmos;

namespace LibSdk3
{
    public sealed class CosmosDbRepository : ICosmosDbRepository
    {
        private readonly Lazy<Database> _database;
        private readonly Lazy<Container> _container;
        private readonly CosmosClient _cosmosDbClient;
        private readonly ICosmosDbSettings _cosmosDbSettings;

        private Database Database => _database.Value;
        private Container Container => _container.Value;

        public CosmosDbRepository(ICosmosDbSettings cosmosDbSettings)
        {
            _cosmosDbClient = new CosmosClient(cosmosDbSettings.EndpointUrl, cosmosDbSettings.AuthorizationKey);
            _cosmosDbSettings = cosmosDbSettings;
            _database = new Lazy<Database>(GetDatabase);
            _container = new Lazy<Container>(GetContainer);
        }

        public async Task<CosmosDbQueryResponse> QueryCosmosDbAsync(CosmosDbQueryRequest request)
        {
            var response = await QueryCosmosDbAsync<dynamic>(request);
            return new CosmosDbQueryResponse(response);
        }

        public async Task<CosmosDbQueryResponse<TDocument>> QueryCosmosDbAsync<TDocument>(CosmosDbQueryRequest request)
        {
            var cosmosDbResponse = new CosmosDbQueryResponse<TDocument>();
            var iterator = Container.GetItemQueryIterator<TDocument>(request.Query, request.CancellationToken, request.Options);
            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                cosmosDbResponse.AddRange(response);
            }
            return cosmosDbResponse;
        }

        public async Task<CosmosDbInsertResponse> InsertCosmosDbAsync(CosmosDbInsertRequest request)
        {
            var options = request.Options;
            var document = request.Document;

            if (document == null)
            {
                return new CosmosDbInsertResponse();
            }

            var partitionKey = request.PartitionKey;
            var itemResponse = await Container.CreateItemAsync<dynamic>(document, new PartitionKey(partitionKey), options);
            return new CosmosDbInsertResponse(itemResponse.RequestCharge, itemResponse.Resource);
        }

        public async Task<CosmosDbDeleteResponse> DeleteCosmosDbAsync(CosmosDbDeleteRequest request)
        {
            var options = request.Options;
            var document = request.Document;

            if (document == null)
            {
                return new CosmosDbDeleteResponse();
            }

            var id = request.Id;
            var partitionKey = request.PartitionKey;
            var itemResponse = await Container.DeleteItemAsync<dynamic>(id, new PartitionKey(partitionKey), options);
            return new CosmosDbDeleteResponse(itemResponse.RequestCharge, itemResponse.Resource);
        }

        public async Task<CosmosDbCreateResponse> CreateCosmosDbAsync(CosmosDbCreateRequest request)
        {
            var databaseResponse = await _cosmosDbClient.CreateDatabaseIfNotExistsAsync(_cosmosDbSettings.DatabaseName, _cosmosDbSettings.DatabaseThroughput);
            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(_cosmosDbSettings.ContainerName, _cosmosDbSettings.PartitionKeyPath);
            var requestCharge = databaseResponse.RequestCharge + containerResponse.RequestCharge;
            return new CosmosDbCreateResponse(requestCharge);
        }

        public async Task<CosmosDbDestroyResponse> DestroyCosmosDbAsync(CosmosDbDestroyRequest request)
        {
            var containerResponse = await Container.DeleteContainerAsync();
            var databaseResponse = await Database.DeleteAsync();
            var requestCharge = databaseResponse.RequestCharge + containerResponse.RequestCharge;
            return new CosmosDbDestroyResponse(requestCharge);
        }

        public void Dispose()
        {
            _cosmosDbClient?.Dispose();
        }

        private Database GetDatabase()
        {
            return _cosmosDbClient.GetDatabase(_cosmosDbSettings.DatabaseName);
        }

        private Container GetContainer()
        {
            return _cosmosDbClient.GetContainer(_cosmosDbSettings.DatabaseName, _cosmosDbSettings.ContainerName);
        }
    }
}
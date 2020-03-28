using System;
using System.Threading.Tasks;
using LibSdk2.Models;
using LibSdk2.Settings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace LibSdk2
{
    public sealed class CosmosDbRepository : ICosmosDbRepository
    {
        private readonly IDocumentClient _documentClient;
        private readonly ICosmosDbSettings _cosmosDbSettings;

        public CosmosDbRepository(ICosmosDbSettings cosmosDbSettings)
        {
            _cosmosDbSettings = cosmosDbSettings;
            _documentClient = new DocumentClient(
                new Uri(cosmosDbSettings.EndpointUrl),
                cosmosDbSettings.AuthorizationKey,
                cosmosDbSettings.ConnectionPolicy);
            ValidateCosmosDbClientAndSettings();
        }

        public async Task<CosmosDbResponse<TDocument>> GetCosmosDbResponseAsync<TDocument>(CosmosDbRequest request)
        {
            var documentUri = UriFactory.CreateDocumentCollectionUri(_cosmosDbSettings.DatabaseName, _cosmosDbSettings.CollectionName);
            var documentQuery = _documentClient.CreateDocumentQuery<TDocument>(documentUri, request.Query, request.Options).AsDocumentQuery();
            var cosmosDbResponse = new CosmosDbResponse<TDocument>();
            while (documentQuery.HasMoreResults)
            {
                var feedResponse = await documentQuery.ExecuteNextAsync<TDocument>();
                cosmosDbResponse.AddRange(feedResponse);
            }
            return cosmosDbResponse;
        }

        public void Dispose()
        {
            (_documentClient as DocumentClient)?.Dispose();
        }

        private void ValidateCosmosDbClientAndSettings()
        {
            (_documentClient as DocumentClient)?.OpenAsync().GetAwaiter().GetResult();
            var collectionUri = UriFactory.CreateDocumentCollectionUri(
                _cosmosDbSettings.DatabaseName,
                _cosmosDbSettings.CollectionName);
            _documentClient.ReadDocumentCollectionAsync(collectionUri).GetAwaiter().GetResult();
        }
    }
}
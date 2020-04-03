using System;
using System.Threading.Tasks;
using LibSdk2.Models.CreateModels;
using LibSdk2.Models.DeleteModels;
using LibSdk2.Models.DestroyModels;
using LibSdk2.Models.InsertModels;
using LibSdk2.Models.QueryModels;
using LibSdk2.Settings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace LibSdk2
{
    public sealed class CosmosDbRepository : ICosmosDbRepository
    {
        private readonly Uri _databaseUri;
        private readonly Uri _collectionUri;
        private readonly DocumentClient _documentClient;
        private readonly ICosmosDbSettings _cosmosDbSettings;

        public CosmosDbRepository(ICosmosDbSettings cosmosDbSettings)
        {
            _cosmosDbSettings = cosmosDbSettings;
            _documentClient = new DocumentClient(
                new Uri(cosmosDbSettings.EndpointUrl),
                cosmosDbSettings.AuthorizationKey,
                cosmosDbSettings.ConnectionPolicy);
            _documentClient.OpenAsync().GetAwaiter().GetResult();
            (_databaseUri, _collectionUri) = CreateCosmosDbUris(cosmosDbSettings);
        }

        public async Task<CosmosDbQueryResponse> QueryCosmosDbAsync(CosmosDbQueryRequest request)
        {
            var response = await QueryCosmosDbAsync<Document>(request);
            return new CosmosDbQueryResponse(response);
        }

        public async Task<CosmosDbQueryResponse<TDocument>> QueryCosmosDbAsync<TDocument>(CosmosDbQueryRequest request)
        {
            var documentQuery = _documentClient.CreateDocumentQuery<TDocument>(_collectionUri, request.Query, request.Options).AsDocumentQuery();
            var cosmosDbResponse = new CosmosDbQueryResponse<TDocument>();
            while (documentQuery.HasMoreResults)
            {
                var feedResponse = await documentQuery.ExecuteNextAsync<TDocument>();
                cosmosDbResponse.AddRange(feedResponse);
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

            var resourceResponse = await _documentClient.CreateDocumentAsync(_collectionUri, document, options);
            return new CosmosDbInsertResponse(resourceResponse.RequestCharge, resourceResponse.Resource);
        }

        public async Task<CosmosDbDeleteResponse> DeleteCosmosDbAsync(CosmosDbDeleteRequest request)
        {
            var options = request.Options;
            var document = request.Document;

            if (document == null)
            {
                return new CosmosDbDeleteResponse();
            }

            var documentId = document.Id;
            var databaseId = _cosmosDbSettings.DatabaseName;
            var collectionId = _cosmosDbSettings.CollectionName;
            var documentUri = UriFactory.CreateDocumentUri(databaseId, collectionId, documentId);
            var resourceResponse = await _documentClient.DeleteDocumentAsync(documentUri, options);
            return new CosmosDbDeleteResponse(resourceResponse.RequestCharge, resourceResponse.Resource);
        }

        public async Task<CosmosDbCreateResponse> CreateCosmosDbAsync(CosmosDbCreateRequest request)
        {
            var database = new Database{ Id = request.DatabaseName };
            var databaseOptions = new RequestOptions { OfferThroughput = request.DatabaseThroughput };
            var databaseResponse = await _documentClient.CreateDatabaseIfNotExistsAsync(database, databaseOptions);
            var collection = new DocumentCollection { Id = request.CollectionName, DefaultTimeToLive = -1 };
            collection.PartitionKey.Paths.Add(request.PartitionKeyName);
            var collectionResponse = await _documentClient.CreateDocumentCollectionIfNotExistsAsync(_databaseUri, collection);
            var requestCharge = databaseResponse.RequestCharge + collectionResponse.RequestCharge;
            return new CosmosDbCreateResponse(requestCharge);
        }

        public async Task<CosmosDbDestroyResponse> DestroyCosmosDbAsync(CosmosDbDestroyRequest request)
        {
            var collectionResponse = await _documentClient.DeleteDocumentCollectionAsync(_collectionUri);
            var databaseResponse = await _documentClient.DeleteDatabaseAsync(_databaseUri);
            var requestCharge = databaseResponse.RequestCharge + collectionResponse.RequestCharge;
            return new CosmosDbDestroyResponse(requestCharge);
        }

        public void Dispose()
        {
            _documentClient.Dispose();
        }

        private static (Uri, Uri) CreateCosmosDbUris(ICosmosDbSettings settings)
        {
            var databaseUri = UriFactory.CreateDatabaseUri(settings.DatabaseName);
            var collectionUri = UriFactory.CreateDocumentCollectionUri(settings.DatabaseName, settings.CollectionName);
            return (databaseUri, collectionUri);
        }
    }
}
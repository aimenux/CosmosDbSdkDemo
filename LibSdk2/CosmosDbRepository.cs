using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace LibSdk2
{
    public sealed class CosmosDbRepository<TDocument> : ICosmosDbRepository<TDocument> where TDocument : ICosmosDbDocument
    {
        private readonly Uri _collectionUri;
        private readonly DocumentClient _documentClient;

        public CosmosDbRepository(CosmosDbSettings settings)
        {
            var endpointUri = new Uri(settings.EndpointUrl);
            _collectionUri = UriFactory.CreateDocumentCollectionUri(settings.DatabaseName, settings.CollectionName);
            _documentClient = new DocumentClient(endpointUri, settings.AuthorizationKey, settings.ConnectionPolicy);
            ValidateCosmosDbConnection();
        }

        public Task<ICollection<TDocument>> GetDocumentsAsync(string query)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _documentClient?.Dispose();
        }

        private void ValidateCosmosDbConnection()
        {
            _documentClient.OpenAsync().GetAwaiter().GetResult();
            _documentClient.ReadDocumentCollectionAsync(_collectionUri).GetAwaiter().GetResult();
        }
    }
}
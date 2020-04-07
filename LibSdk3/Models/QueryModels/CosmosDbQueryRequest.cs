using LibSdk3.Settings;
using Microsoft.Azure.Cosmos;

namespace LibSdk3.Models.QueryModels
{
    public class CosmosDbQueryRequest
    {
        public string Query { get; }

        public QueryRequestOptions Options { get; }

        public string CancellationToken { get; set; }

        public CosmosDbQueryRequest(string collectionName, string partitionKey)
        {
            Query = $@"SELECT * FROM {collectionName}";
            Options = new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            };
        }

        public CosmosDbQueryRequest(string collectionName, string partitionKey, string id)
        {
            Query = $@"SELECT * FROM {collectionName} WHERE {collectionName}.id = '{id}'";
            Options = new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            };
        }

        public CosmosDbQueryRequest(string query, QueryRequestOptions options = null)
        {
            Query = query;
            Options = options;
        }

        public CosmosDbQueryRequest(CosmosDbQuery cosmosDbQuery) : this(cosmosDbQuery.Query)
        {
        }
    }
}
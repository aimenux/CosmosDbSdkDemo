using LibSdk2.Settings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace LibSdk2.Models.QueryModels
{
    public class CosmosDbQueryRequest
    {
        public string Query { get; }
        public FeedOptions Options { get; }

        public CosmosDbQueryRequest(string collectionName, string partitionKey)
        {
            Query = $@"SELECT * FROM {collectionName}";
            Options = new FeedOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            };
        }

        public CosmosDbQueryRequest(string collectionName, string partitionKey, string id)
        {
            Query = $@"SELECT * FROM {collectionName} WHERE {collectionName}.id = '{id}'";
            Options = new FeedOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            };
        }

        public CosmosDbQueryRequest(string query, FeedOptions options = null)
        {
            Query = query;
            Options = options;
        }

        public CosmosDbQueryRequest(string query, bool enableCrossPartition) : this(query)
        {
            Options = new FeedOptions
            {
                EnableCrossPartitionQuery = enableCrossPartition
            };
        }

        public CosmosDbQueryRequest(CosmosDbQuery cosmosDbQuery) : this(cosmosDbQuery.Query, cosmosDbQuery.EnableCrossPartition)
        {
        }
    }
}
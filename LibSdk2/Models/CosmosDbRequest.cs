using LibSdk2.Settings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace LibSdk2.Models
{
    public class CosmosDbRequest
    {
        public string Query { get; }
        public FeedOptions Options { get; }

        public CosmosDbRequest(string collectionName, string id, string partitionKey)
        {
            Query = $@"SELECT * FROM {collectionName} WHERE {collectionName}.id = '{id}'";
            Options = new FeedOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            };
        }

        public CosmosDbRequest(string query, FeedOptions options = null)
        {
            Query = query;
            Options = options;
        }

        public CosmosDbRequest(string query, bool enableCrossPartition) : this(query)
        {
            Options = new FeedOptions
            {
                EnableCrossPartitionQuery = enableCrossPartition
            };
        }

        public CosmosDbRequest(CosmosDbQuery cosmosDbQuery) : this(cosmosDbQuery.Query, cosmosDbQuery.EnableCrossPartition)
        {
        }
    }
}
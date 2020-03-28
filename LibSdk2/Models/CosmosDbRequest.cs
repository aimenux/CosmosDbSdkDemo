﻿using Microsoft.Azure.Documents.Client;

namespace LibSdk2.Models
{
    public class CosmosDbRequest
    {
        public string Query { get; }
        public FeedOptions Options { get; }

        public CosmosDbRequest(string query, bool enableCrossPartition)
        {
            Query = query;
            Options = new FeedOptions
            {
                EnableCrossPartitionQuery = enableCrossPartition
            };
        }
    }
}
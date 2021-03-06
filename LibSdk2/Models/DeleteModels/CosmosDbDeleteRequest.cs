﻿using System.IO;
using LibSdk2.Settings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace LibSdk2.Models.DeleteModels
{
    public class CosmosDbDeleteRequest<TDocument>
    {
        public TDocument Document { get; set; }
        public RequestOptions Options { get; set; }
    }

    public class CosmosDbDeleteRequest : CosmosDbDeleteRequest<Document>
    {
        public CosmosDbDeleteRequest(CosmosDbDocument cosmosDbDocument, ICosmosDbSettings settings)
        {
            var json = cosmosDbDocument.Document;
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                Document = new Document();
                Document.LoadFrom(reader);
            }

            var pkName = settings.PartitionKeyName;
            var pkValue = Document.GetPropertyValue<object>(pkName);
            Options = new RequestOptions
            {
                PartitionKey = new PartitionKey(pkValue)
            };
        }
    }
}
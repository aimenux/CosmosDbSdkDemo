using System.IO;
using LibSdk2.Settings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace LibSdk2.Models.InsertModels
{
    public class CosmosDbInsertRequest<TDocument>
    {
        public TDocument Document { get; protected set; }
        public RequestOptions Options { get; }

        public CosmosDbInsertRequest(TDocument document = default, RequestOptions options = null)
        {
            Document = document;
            Options = options;
        }
    }

    public class CosmosDbInsertRequest : CosmosDbInsertRequest<Document>
    {
        public CosmosDbInsertRequest(CosmosDbDocument cosmosDbDocument)
        {
            var json = cosmosDbDocument.Document;
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                Document = new Document();
                Document.LoadFrom(reader);
            }
        }
    }
}
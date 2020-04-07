using LibSdk3.Settings;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibSdk3.Models.InsertModels
{
    public class CosmosDbInsertRequest<TDocument>
    {
        public string PartitionKey { get; set; }
        public TDocument Document { get; set; }
        public ItemRequestOptions Options { get; set; }
    }

    public class CosmosDbInsertRequest : CosmosDbInsertRequest<dynamic>
    {
        public CosmosDbInsertRequest(CosmosDbDocument cosmosDbDocument, ICosmosDbSettings settings)
        {
            var json = cosmosDbDocument.Document;
            var jsonObject = JObject.Parse(json);
            PartitionKey = jsonObject[settings.PartitionKeyName].ToString();
            Document = JsonConvert.DeserializeObject(json);
        }
    }
}
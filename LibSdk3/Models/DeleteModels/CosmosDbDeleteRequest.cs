using LibSdk3.Settings;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibSdk3.Models.DeleteModels
{
    public class CosmosDbDeleteRequest<TDocument>
    {
        public string Id { get; set; }
        public string PartitionKey { get; set; }
        public TDocument Document { get; set; }
        public ItemRequestOptions Options { get; set; }
    }

    public class CosmosDbDeleteRequest : CosmosDbDeleteRequest<dynamic>
    {
        public CosmosDbDeleteRequest(CosmosDbDocument cosmosDbDocument, ICosmosDbSettings settings)
        {
            var json = cosmosDbDocument.Document;
            var jsonObject = JObject.Parse(json);
            Id = jsonObject["id"].ToString();
            PartitionKey = jsonObject[settings.PartitionKeyName].ToString();
            Document = JsonConvert.DeserializeObject(json);
        }
    }
}
using LibSdk2.Settings;

namespace LibSdk2.Models.CreateModels
{
    public class CosmosDbCreateRequest
    {
        public string DatabaseName { get; }
        public string CollectionName { get; }
        public int DatabaseThroughput { get; }
        public string PartitionKeyName { get; }

        public CosmosDbCreateRequest(ICosmosDbSettings settings)
        {
            DatabaseName = settings.DatabaseName;
            CollectionName = settings.CollectionName;
            DatabaseThroughput = settings.DatabaseThroughput;
            PartitionKeyName = settings.PartitionKeyName;
        }
    }
}
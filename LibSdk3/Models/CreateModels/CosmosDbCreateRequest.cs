using LibSdk3.Settings;

namespace LibSdk3.Models.CreateModels
{
    public class CosmosDbCreateRequest
    {
        public string DatabaseName { get; }
        public string ContainerName { get; }
        public int DatabaseThroughput { get; }
        public string PartitionKeyName { get; }

        public CosmosDbCreateRequest(ICosmosDbSettings settings)
        {
            DatabaseName = settings.DatabaseName;
            ContainerName = settings.ContainerName;
            DatabaseThroughput = settings.DatabaseThroughput;
            PartitionKeyName = settings.PartitionKeyName;
        }
    }
}
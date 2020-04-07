using System;

namespace LibSdk3.Settings
{
    public class CosmosDbSettings : ICosmosDbSettings
    {
        public string EndpointUrl { get; set; }
        public string AuthorizationKey { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public int DatabaseThroughput { get; set; }
        public string PartitionKeyPath { get; set; }
        public string PartitionKeyName => GetPartitionKeyName();

        private string GetPartitionKeyName()
        {
            var path = PartitionKeyPath;
            var pos = path.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;
            var name = path.Substring(pos, path.Length - pos);
            return name;
        }
    }
}
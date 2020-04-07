namespace LibSdk3.Settings
{
    public interface ICosmosDbSettings
    {
        string EndpointUrl { get; set; }
        string AuthorizationKey { get; set; }
        string DatabaseName { get; set; }
        string ContainerName { get; set; }
        int DatabaseThroughput { get; set; }
        string PartitionKeyPath { get; set; }
        string PartitionKeyName { get; }
    }
}
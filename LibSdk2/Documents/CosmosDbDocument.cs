namespace LibSdk2.Documents
{
    public class CosmosDbDocument : ICosmosDbDocument
    {
        public string Id { get; set; }
        public string PartitionKey { get; set; }
    }
}
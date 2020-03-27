namespace LibSdk2.Documents
{
    public interface ICosmosDbDocument
    {
        string Id { get; set; }
        string PartitionKey { get; set; }
    }
}
namespace LibSdk2
{
    public interface ICosmosDbDocument
    {
        string Id { get; set; }
        string PartitionKey { get; set; }
    }
}
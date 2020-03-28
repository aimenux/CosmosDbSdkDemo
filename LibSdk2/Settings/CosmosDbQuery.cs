namespace LibSdk2.Settings
{
    public class CosmosDbQuery : ICosmosDbQuery
    {
        public string Query { get; set; }
        public bool EnableCrossPartition { get; set; }
    }
}
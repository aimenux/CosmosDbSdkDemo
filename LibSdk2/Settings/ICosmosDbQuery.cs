namespace LibSdk2.Settings
{
    public interface ICosmosDbQuery
    {
        string Query { get; set; }
        bool EnableCrossPartition { get; set; }
    }
}

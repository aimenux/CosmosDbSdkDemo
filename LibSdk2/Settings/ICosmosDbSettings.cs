using Microsoft.Azure.Documents.Client;

namespace LibSdk2.Settings
{
    public interface ICosmosDbSettings
    {
        string EndpointUrl { get; set; }
        string AuthorizationKey { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
        ConnectionPolicy ConnectionPolicy { get; set; }
    }
}
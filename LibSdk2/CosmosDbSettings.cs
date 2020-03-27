using Microsoft.Azure.Documents.Client;

namespace LibSdk2
{
    public class CosmosDbSettings
    {
        public string EndpointUrl { get; set; }
        public string AuthorizationKey { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
        public ConnectionPolicy ConnectionPolicy { get; set; }

        public CosmosDbSettings()
        {
            ConnectionPolicy = new ConnectionPolicy
            {
                ConnectionProtocol = Protocol.Tcp,
                ConnectionMode = ConnectionMode.Direct
            };
        }
    }
}
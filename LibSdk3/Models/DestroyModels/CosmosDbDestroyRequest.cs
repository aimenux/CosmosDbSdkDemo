using LibSdk3.Settings;

namespace LibSdk3.Models.DestroyModels
{
    public class CosmosDbDestroyRequest
    {
        public string DatabaseName { get; }
        public string ContainerName { get; }

        public CosmosDbDestroyRequest(ICosmosDbSettings settings)
        {
            DatabaseName = settings.DatabaseName;
            ContainerName = settings.ContainerName;
        }
    }
}
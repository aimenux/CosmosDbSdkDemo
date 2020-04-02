using LibSdk2.Models.CreateModels;
using LibSdk2.Models.DestroyModels;
using LibSdk2.Models.InsertModels;
using LibSdk2.Models.QueryModels;

namespace LibSdk2
{
    public interface ICosmosDbPrinter
    {
        void Print(CosmosDbQueryRequest request, CosmosDbQueryResponse response);
        void Print(CosmosDbInsertRequest request, CosmosDbInsertResponse response);
        void Print(CosmosDbCreateRequest request, CosmosDbCreateResponse response);
        void Print(CosmosDbDestroyRequest request, CosmosDbDestroyResponse response);
    }
}
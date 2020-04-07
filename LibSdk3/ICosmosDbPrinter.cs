using LibSdk3.Models.CreateModels;
using LibSdk3.Models.DeleteModels;
using LibSdk3.Models.DestroyModels;
using LibSdk3.Models.InsertModels;
using LibSdk3.Models.QueryModels;

namespace LibSdk3
{
    public interface ICosmosDbPrinter
    {
        void Print(CosmosDbQueryRequest request, CosmosDbQueryResponse response);
        void Print(CosmosDbInsertRequest request, CosmosDbInsertResponse response);
        void Print(CosmosDbDeleteRequest request, CosmosDbDeleteResponse response);
        void Print(CosmosDbCreateRequest request, CosmosDbCreateResponse response);
        void Print(CosmosDbDestroyRequest request, CosmosDbDestroyResponse response);
    }
}
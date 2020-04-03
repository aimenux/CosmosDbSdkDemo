using System;
using System.Threading.Tasks;
using LibSdk2.Models.CreateModels;
using LibSdk2.Models.DeleteModels;
using LibSdk2.Models.DestroyModels;
using LibSdk2.Models.InsertModels;
using LibSdk2.Models.QueryModels;

namespace LibSdk2
{
    public interface ICosmosDbRepository : IDisposable
    {
        Task<CosmosDbQueryResponse> QueryCosmosDbAsync(CosmosDbQueryRequest request);
        Task<CosmosDbQueryResponse<TDocument>> QueryCosmosDbAsync<TDocument>(CosmosDbQueryRequest request);
        Task<CosmosDbInsertResponse> InsertCosmosDbAsync(CosmosDbInsertRequest request);
        Task<CosmosDbDeleteResponse> DeleteCosmosDbAsync(CosmosDbDeleteRequest request);
        Task<CosmosDbCreateResponse> CreateCosmosDbAsync(CosmosDbCreateRequest request);
        Task<CosmosDbDestroyResponse> DestroyCosmosDbAsync(CosmosDbDestroyRequest request);
    }
}

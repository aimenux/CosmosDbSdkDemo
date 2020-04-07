using System;
using System.Threading.Tasks;
using LibSdk3.Models.CreateModels;
using LibSdk3.Models.DeleteModels;
using LibSdk3.Models.DestroyModels;
using LibSdk3.Models.InsertModels;
using LibSdk3.Models.QueryModels;

namespace LibSdk3
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

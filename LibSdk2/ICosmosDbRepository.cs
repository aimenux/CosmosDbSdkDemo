using System;
using System.Threading.Tasks;
using LibSdk2.Models;
using Microsoft.Azure.Documents.Client;

namespace LibSdk2
{
    public interface ICosmosDbRepository : IDisposable
    {
        Task<CosmosDbResponse<TDocument>> GetCosmosDbResponseAsync<TDocument>(CosmosDbRequest request);
    }
}

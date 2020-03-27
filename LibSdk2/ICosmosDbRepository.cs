using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace LibSdk2
{
    public interface ICosmosDbRepository : IDisposable
    {
        Task<ICollection<TDocument>> GetDocumentsAsync<TDocument>(string query, FeedOptions options = null);
    }
}

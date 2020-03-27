using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibSdk2
{
    public interface ICosmosDbRepository<TDocument> : IDisposable where TDocument : ICosmosDbDocument
    {
        Task<ICollection<TDocument>> GetDocumentsAsync(string query);
    }
}

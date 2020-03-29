using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;

namespace LibSdk2.Models.QueryModels
{
    public class CosmosDbQueryResponse<TDocument>
    {
        public double RequestUnits { get; private set; }
        public ICollection<TDocument> Documents { get; }

        public CosmosDbQueryResponse(ICollection<TDocument> documents = null)
        {
            RequestUnits = 0f;
            Documents = documents ?? new List<TDocument>();
        }

        public void AddRange(params FeedResponse<TDocument>[] responses)
        {
            if (responses == null) return;
            foreach (var response in responses)
            {
                RequestUnits += response.RequestCharge;
                foreach (var document in response)
                {
                    Documents.Add(document);
                }
            }
        }
    }

    public class CosmosDbQueryResponse : CosmosDbQueryResponse<dynamic>
    {
    }
}

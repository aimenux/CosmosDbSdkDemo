using System.Collections.Generic;
using System.Linq;

namespace LibSdk3.Models.DeleteModels
{
    public class CosmosDbDeleteResponse<TDocument>
    {
        public double RequestUnits { get; }
        public ICollection<TDocument> DeletedDocuments { get; }

        public CosmosDbDeleteResponse(double requestUnits = 0f, params TDocument[] deletedDocuments)
        {
            RequestUnits = requestUnits;
            DeletedDocuments = deletedDocuments?.ToList() ?? new List<TDocument>();
        }
    }

    public class CosmosDbDeleteResponse : CosmosDbDeleteResponse<dynamic>
    {
        public CosmosDbDeleteResponse(double requestUnits = 0f, params dynamic[] deletedDocuments) : base(requestUnits, deletedDocuments)
        {
        }
    }
}

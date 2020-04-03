using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;

namespace LibSdk2.Models.DeleteModels
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

    public class CosmosDbDeleteResponse : CosmosDbDeleteResponse<Document>
    {
        public CosmosDbDeleteResponse(double requestUnits = 0f, params Document[] deletedDocuments) : base(requestUnits, deletedDocuments)
        {
        }
    }
}

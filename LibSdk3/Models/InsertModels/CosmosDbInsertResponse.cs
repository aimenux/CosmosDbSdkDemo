using System.Collections.Generic;
using System.Linq;

namespace LibSdk3.Models.InsertModels
{
    public class CosmosDbInsertResponse<TDocument>
    {
        public double RequestUnits { get; }
        public ICollection<TDocument> InsertedDocuments { get; }

        public CosmosDbInsertResponse(double requestUnits = 0f, params TDocument[] insertedDocuments)
        {
            RequestUnits = requestUnits;
            InsertedDocuments = insertedDocuments?.ToList() ?? new List<TDocument>();
        }
    }

    public class CosmosDbInsertResponse : CosmosDbInsertResponse<dynamic>
    {
        public CosmosDbInsertResponse(double requestUnits = 0f, params dynamic[] insertedDocuments) : base(requestUnits, insertedDocuments)
        {
        }
    }
}

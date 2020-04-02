using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents;

namespace LibSdk2.Models.InsertModels
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

    public class CosmosDbInsertResponse : CosmosDbInsertResponse<Document>
    {
        public CosmosDbInsertResponse(double requestUnits = 0f, params Document[] insertedDocuments) : base(requestUnits, insertedDocuments)
        {
        }
    }
}

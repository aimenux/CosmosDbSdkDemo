using System;
using LibSdk3.Models.CreateModels;
using LibSdk3.Models.DeleteModels;
using LibSdk3.Models.DestroyModels;
using LibSdk3.Models.InsertModels;
using LibSdk3.Models.QueryModels;

namespace LibSdk3
{
    public class CosmosDbPrinter : ICosmosDbPrinter
    {
        public void Print(CosmosDbQueryRequest request, CosmosDbQueryResponse response)
        {
            Console.WriteLine($"Query: {request.Query}");
            Console.WriteLine($"Found: {response.Documents.Count} document(s)");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }

        public void Print(CosmosDbInsertRequest request, CosmosDbInsertResponse response)
        {
            Console.WriteLine($"Document: {request.Document}");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }

        public void Print(CosmosDbDeleteRequest request, CosmosDbDeleteResponse response)
        {
            Console.WriteLine($"Document: {request.Document}");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }

        public void Print(CosmosDbCreateRequest request, CosmosDbCreateResponse response)
        {
            Console.WriteLine($"DatabaseName: {request.DatabaseName}");
            Console.WriteLine($"ContainerName: {request.ContainerName}");
            Console.WriteLine($"DatabaseThroughput: {request.DatabaseThroughput}");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }

        public void Print(CosmosDbDestroyRequest request, CosmosDbDestroyResponse response)
        {
            Console.WriteLine($"DatabaseName: {request.DatabaseName}");
            Console.WriteLine($"ContainerName: {request.ContainerName}");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }
    }
}
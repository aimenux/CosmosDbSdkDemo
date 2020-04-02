using System;
using LibSdk2.Models.CreateModels;
using LibSdk2.Models.DestroyModels;
using LibSdk2.Models.InsertModels;
using LibSdk2.Models.QueryModels;

namespace LibSdk2
{
    public class CosmosDbPrinter : ICosmosDbPrinter
    {
        public void Print(CosmosDbQueryRequest request, CosmosDbQueryResponse response)
        {
            Console.WriteLine($"Query: {request.Query}");
            Console.WriteLine($"EnableCrossPartition: {request.Options.EnableCrossPartitionQuery}");
            Console.WriteLine($"Found: {response.Documents.Count} document(s)");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }

        public void Print(CosmosDbInsertRequest request, CosmosDbInsertResponse response)
        {
            Console.WriteLine($"Document: {request.Document}");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }

        public void Print(CosmosDbCreateRequest request, CosmosDbCreateResponse response)
        {
            Console.WriteLine($"DatabaseName: {request.DatabaseName}");
            Console.WriteLine($"CollectionName: {request.CollectionName}");
            Console.WriteLine($"DatabaseThroughput: {request.DatabaseThroughput}");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }

        public void Print(CosmosDbDestroyRequest request, CosmosDbDestroyResponse response)
        {
            Console.WriteLine($"DatabaseName: {request.DatabaseName}");
            Console.WriteLine($"CollectionName: {request.CollectionName}");
            Console.WriteLine($"RequestUnits: {response.RequestUnits} RU");
        }
    }
}
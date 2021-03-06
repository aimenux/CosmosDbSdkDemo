﻿using LibSdk2.Settings;

namespace LibSdk2.Models.CreateModels
{
    public class CosmosDbCreateRequest
    {
        public string DatabaseName { get; }
        public string ContainerName { get; }
        public int DatabaseThroughput { get; }
        public string PartitionKeyPath { get; }

        public CosmosDbCreateRequest(ICosmosDbSettings settings)
        {
            DatabaseName = settings.DatabaseName;
            ContainerName = settings.ContainerName;
            DatabaseThroughput = settings.DatabaseThroughput;
            PartitionKeyPath = settings.PartitionKeyPath;
        }
    }
}
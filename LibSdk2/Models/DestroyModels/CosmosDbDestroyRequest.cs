﻿using LibSdk2.Settings;

namespace LibSdk2.Models.DestroyModels
{
    public class CosmosDbDestroyRequest
    {
        public string DatabaseName { get; }
        public string ContainerName { get; }

        public CosmosDbDestroyRequest(ICosmosDbSettings settings)
        {
            DatabaseName = settings.DatabaseName;
            ContainerName = settings.ContainerName;
        }
    }
}
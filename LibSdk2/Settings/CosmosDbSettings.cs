﻿using System;
using Microsoft.Azure.Documents.Client;

namespace LibSdk2.Settings
{
    public class CosmosDbSettings : ICosmosDbSettings
    {
        public string EndpointUrl { get; set; }
        public string AuthorizationKey { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public int DatabaseThroughput { get; set; }
        public string PartitionKeyPath { get; set; }
        public string PartitionKeyName => GetPartitionKeyName();
        public ConnectionPolicy ConnectionPolicy { get; set; }

        public CosmosDbSettings()
        {
            ConnectionPolicy = new ConnectionPolicy
            {
                ConnectionProtocol = Protocol.Tcp,
                ConnectionMode = ConnectionMode.Direct
            };
        }

        private string GetPartitionKeyName()
        {
            var path = PartitionKeyPath;
            var pos = path.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1;
            var name = path.Substring(pos, path.Length - pos);
            return name;
        }
    }
}
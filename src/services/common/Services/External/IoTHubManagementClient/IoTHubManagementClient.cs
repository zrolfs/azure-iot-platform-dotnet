// <copyright file="IoTHubManagementClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.Azure.Management.IotHub;
using Microsoft.Azure.Management.IotHub.Models;
using Microsoft.Rest.Azure;
using Microsoft.Rest.ClientRuntime;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Models;

namespace Mmm.Iot.Common.Services.External.TableStorage
{
    public class IoTHubManagementClient : IIoTHubManagementClient
    {
        private readonly IotHubClient client;
        private readonly AppConfig config;

        public IoTHubManagementClient(IIoTHubManagementClientFactory clientFactory, AppConfig config)
        {
            this.client = clientFactory.Create();
            this.config = config;
        }

        public async Task<StatusResultServiceModel> StatusAsync()
        {
            try
            {
                var result = this.client.IsDefined();
                await Task.CompletedTask; // Just to keep the signature async, later this should be replaced with more robust status check

                // If the call above does not fail then return a healthy status
                return new StatusResultServiceModel(result, result ? "Alive and well!" : "Undefined IoTHubClient");
            }
            catch (Exception e)
            {
                return new StatusResultServiceModel(false, $"Table Storage status check failed: {e.Message}");
            }
        }

        public async Task<IotHubDescription> RetrieveAsync(string iotHubName, CancellationToken token)
        {
            return await this.client.IotHubResource.GetAsync(this.config.Global.ResourceGroup, iotHubName, token != null ? token : CancellationToken.None);
        }

        public IPage<SharedAccessSignatureAuthorizationRule> ListKeysAsync(string iotHubName)
        {
            return this.client.IotHubResource.ListKeys(this.config.Global.ResourceGroup, iotHubName);
        }

        public string GetConnectionString(string iotHubName)
        {
            var keys = this.ListKeysAsync(iotHubName);
            return $"HostName={iotHubName}.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey={keys.Where(t => t.KeyName == "iothubowner").FirstOrDefault().PrimaryKey}";
        }
    }
}
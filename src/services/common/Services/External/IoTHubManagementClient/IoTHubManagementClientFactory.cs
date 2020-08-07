// <copyright file="IoTHubManagementClientFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Management.IotHub;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Mmm.Iot.Common.Services.Config;

namespace Mmm.Iot.Common.Services.External.TableStorage
{
    public class IoTHubManagementClientFactory : IIoTHubManagementClientFactory
    {
        private readonly AppConfig config;

        public IoTHubManagementClientFactory(AppConfig config)
        {
            this.config = config;
        }

        public IotHubClient Create()
        {
            var servicePrincipal = new ServicePrincipalLoginInformation();
            servicePrincipal.ClientId = this.config.Global.AzureActiveDirectory.AppId;
            servicePrincipal.ClientSecret = this.config.Global.AzureActiveDirectory.AppSecret;
            AzureCredentials creds = new AzureCredentials(servicePrincipal, this.config.Global.AzureActiveDirectory.TenantId, AzureEnvironment.AzureGlobalCloud);
            IotHubClient iothubClient = new IotHubClient(creds);
            iothubClient.SubscriptionId = this.config.Global.SubscriptionId;
            return iothubClient;
        }
    }
}
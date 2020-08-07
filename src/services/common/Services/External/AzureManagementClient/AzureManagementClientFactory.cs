// <copyright file="AzureManagementClientFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Management.IotHub;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Mmm.Iot.Common.Services.Config;

namespace Mmm.Iot.Common.Services.External.TableStorage
{
    public class AzureManagementClientFactory : IAzureManagementClientFactory
    {
        private readonly AppConfig config;

        public AzureManagementClientFactory(AppConfig config)
        {
            this.config = config;
        }

        public Microsoft.Azure.Management.Fluent.IAzure Create()
        {
            var servicePrincipal = new ServicePrincipalLoginInformation();
            servicePrincipal.ClientId = this.config.Global.AzureActiveDirectory.AppId;
            servicePrincipal.ClientSecret = this.config.Global.AzureActiveDirectory.AppSecret;
            AzureCredentials creds = new AzureCredentials(servicePrincipal, this.config.Global.AzureActiveDirectory.TenantId, AzureEnvironment.AzureGlobalCloud);
            var azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .Authenticate(creds)
                .WithSubscription(this.config.Global.SubscriptionId);
            return azure;
        }
    }
}
// <copyright file="AzureManagementClient.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.IotHub;
using Microsoft.Azure.Management.IotHub.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Rest.Azure;
using Microsoft.Rest.ClientRuntime;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Models;
using Newtonsoft.Json.Linq;

namespace Mmm.Iot.Common.Services.External.TableStorage
{
    public class AzureManagementClient : IAzureManagementClient
    {
        private readonly IAzure client;
        private readonly AppConfig config;

        public AzureManagementClient(IAzureManagementClientFactory clientFactory, AppConfig config)
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
                return new StatusResultServiceModel(result, result ? "Alive and well!" : "Undefined Azure");
            }
            catch (Exception e)
            {
                return new StatusResultServiceModel(false, $"Table Storage status check failed: {e.Message}");
            }
        }

        public void DeployTemplateAsync(string template, string resourceGroup = null, string deploymentName = null)
        {
            template = JObject.Parse(template).ToString();
            if (resourceGroup == null)
            {
                resourceGroup = this.config.Global.ResourceGroup;
            }

            if (deploymentName == null)
            {
                deploymentName = Guid.Empty.ToString();
            }

            this.client.Deployments.Define(deploymentName)
                .WithExistingResourceGroup(resourceGroup)
                .WithTemplate(template)
                .WithParameters("{}")
                .WithMode(DeploymentMode.Incremental)
                .CreateAsync();
        }
    }
}
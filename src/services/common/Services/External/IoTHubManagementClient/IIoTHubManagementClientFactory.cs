// <copyright file="IIoTHubManagementClientFactory.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Management.IotHub;

namespace Mmm.Iot.Common.Services.External.TableStorage
{
    public interface IIoTHubManagementClientFactory
    {
        IotHubClient Create();
    }
}
// <copyright file="HealthProbeTelemetryProcessor.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Mmm.Iot.Common.Services.AIPreprocessors
{
    public class HealthProbeTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor next;

        public HealthProbeTelemetryProcessor(ITelemetryProcessor next)
        {
            this.next = next;
        }

        public void Process(ITelemetry item)
        {
            if (item is RequestTelemetry)
            {
                var req = item as RequestTelemetry;
                if (req != null && (req.Url.AbsoluteUri.Contains("/ping") ||
                    req.Url.AbsoluteUri.Contains("v1/status")))
                {
                    return;
                }
            }
            else if (item is TraceTelemetry trace)
            {
                if (trace.Properties.TryGetValue("RequestPath", out var requestPath) &&
                    (requestPath == "/v1/status/ping" || requestPath == "/v1/status"))
                {
                    return;
                }
            }
            else if (item is DependencyTelemetry)
            {
                var dependency = item as DependencyTelemetry;
                if (dependency.Context.Operation.Name != null && dependency.Context.Operation.Name.Contains("Status/Get"))
                {
                    return;
                }
            }

            // Send everything else
            this.next.Process(item);
        }
    }
}
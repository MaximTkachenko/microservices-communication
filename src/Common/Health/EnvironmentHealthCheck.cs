using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Common.Health
{
    public class EnvHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"OS_VERSION", Environment.OSVersion.VersionString}, 
                {"MACHINE_NAME", Environment.MachineName}
            };
            foreach (DictionaryEntry variable in Environment.GetEnvironmentVariables())
            {
                dictionary.Add(variable.Key.ToString(), variable.Value);
            }

            return Task.FromResult(HealthCheckResult.Healthy("environment", dictionary));
        }
    }
}

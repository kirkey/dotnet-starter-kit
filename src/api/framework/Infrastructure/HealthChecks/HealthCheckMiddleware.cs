using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FSH.Framework.Infrastructure.HealthChecks;

public class HealthCheckMiddleware(HealthCheckService healthCheckService)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var report = await healthCheckService.CheckHealthAsync().ConfigureAwait(false);

        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                description = entry.Value.Description
            }),
            duration = report.TotalDuration
        };

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(response)).ConfigureAwait(false);
    }
}


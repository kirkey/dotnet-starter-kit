using Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports;

/// <summary>
/// Endpoint configuration for Regulatory Reports module.
/// </summary>
public static class RegulatoryReportsEndpoints
{
    /// <summary>
    /// Maps all Regulatory Reports endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapRegulatoryReportsEndpoints(this IEndpointRouteBuilder app)
    {
        var regulatoryReportsGroup = app.MapGroup("/regulatory-reports")
            .WithTags("Regulatory-Reports")
            .WithDescription("Endpoints for managing regulatory reports");

        // Version 1 endpoints
        regulatoryReportsGroup.MapRegulatoryReportCreateEndpoint();
        regulatoryReportsGroup.MapRegulatoryReportUpdateEndpoint();
        regulatoryReportsGroup.MapRegulatoryReportGetEndpoint();

        return app;
    }
}

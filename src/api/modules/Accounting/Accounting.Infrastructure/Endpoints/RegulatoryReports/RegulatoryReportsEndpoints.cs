using Accounting.Infrastructure.Endpoints.RegulatoryReports.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.RegulatoryReports;

/// <summary>
/// Endpoint configuration for RegulatoryReports module.
/// Provides comprehensive REST API endpoints for managing regulatory-reports.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class RegulatoryReportsEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all RegulatoryReports endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/regulatory-reports").WithTags("regulatory-report");

        group.MapRegulatoryReportCreateEndpoint();
        group.MapRegulatoryReportGetEndpoint();
        group.MapRegulatoryReportSearchEndpoint();
        group.MapRegulatoryReportUpdateEndpoint();
    }
}

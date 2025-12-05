using Accounting.Infrastructure.Endpoints.Checks.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Checks;

/// <summary>
/// Endpoint configuration for Checks module.
/// Provides comprehensive REST API endpoints for managing checks.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class ChecksEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all Checks endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/checks").WithTags("check");

        group.MapCheckClearEndpoint();
        group.MapCheckCreateEndpoint();
        group.MapCheckEndpoints();
        group.MapCheckGetEndpoint();
        group.MapCheckIssueEndpoint();
        group.MapCheckPrintEndpoint();
        group.MapCheckSearchEndpoint();
        group.MapCheckStopPaymentEndpoint();
        group.MapCheckUpdateEndpoint();
        group.MapCheckVoidEndpoint();
    }
}

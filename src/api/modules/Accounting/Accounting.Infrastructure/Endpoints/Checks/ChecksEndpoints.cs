using Accounting.Infrastructure.Endpoints.Checks.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Checks;

/// <summary>
/// Carter module for Checks endpoints.
/// Routes all requests to separate versioned endpoint handlers.
/// </summary>
public class ChecksEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Checks endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/checks").WithTags("checks");

        // Query endpoints
        group.MapCheckGetEndpoint();
        group.MapCheckSearchEndpoint();

        // Command endpoints - CRUD operations
        group.MapCheckCreateEndpoint();
        group.MapCheckUpdateEndpoint();

        // Command endpoints - State transitions
        group.MapCheckIssueEndpoint();
        group.MapCheckVoidEndpoint();
        group.MapCheckClearEndpoint();
        group.MapCheckStopPaymentEndpoint();
        group.MapCheckPrintEndpoint();
    }
}

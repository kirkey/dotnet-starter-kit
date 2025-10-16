namespace Accounting.Infrastructure.Endpoints.Checks.v1;

/// <summary>
/// Route group configuration for check endpoints.
/// Provides comprehensive REST API endpoints for managing checks including creation, issuance, payment stops, and reconciliation.
/// </summary>
public static class CheckEndpoints
{
    /// <summary>
    /// Maps all Check endpoints to the route builder.
    /// Includes Create, Read, Search, and state transition operations (Issue, Void, Clear, Stop Payment, Print).
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The configured endpoint route builder.</returns>
    public static IEndpointRouteBuilder MapCheckEndpointsV1(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/checks")
            .WithTags("Checks")
            .WithDescription("Endpoints for managing checks in the accounting system")
            .MapToApiVersion(1);

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

        return app;
    }
}



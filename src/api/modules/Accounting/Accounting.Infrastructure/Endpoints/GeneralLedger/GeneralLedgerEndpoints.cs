using Accounting.Infrastructure.Endpoints.GeneralLedger.v1;

namespace Accounting.Infrastructure.Endpoints.GeneralLedger;

/// <summary>
/// Endpoint configuration for General Ledger module.
/// </summary>
public static class GeneralLedgerEndpoints
{
    /// <summary>
    /// Maps all General Ledger endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapGeneralLedgerEndpoints(this IEndpointRouteBuilder app)
    {
        var generalLedgerGroup = app.MapGroup("/general-ledger")
            .WithTags("General-Ledger")
            .WithDescription("Endpoints for managing general ledger entries");

        // Version 1 endpoints - Read and Update operations
        // Note: GL entries are primarily created through Journal Entry posting
        generalLedgerGroup.MapGeneralLedgerGetEndpoint();
        generalLedgerGroup.MapGeneralLedgerSearchEndpoint();
        generalLedgerGroup.MapGeneralLedgerUpdateEndpoint();
        // Delete is intentionally not exposed - use reversing entries instead

        return app;
    }
}

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

        // Version 1 endpoints
        // generalLedgerGroup.MapGeneralLedgerUpdateEndpoint();
        // generalLedgerGroup.MapGeneralLedgerDeleteEndpoint();

        return app;
    }
}

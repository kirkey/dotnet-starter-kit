using Accounting.Infrastructure.Endpoints.GeneralLedger.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.GeneralLedger;

/// <summary>
/// Endpoint configuration for GeneralLedger module.
/// Provides comprehensive REST API endpoints for managing general-ledger.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class GeneralLedgerEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all GeneralLedger endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/general-ledger").WithTags("general-ledger");

        // TODO: Implement GeneralLedger Create endpoint
        // group.MapGeneralLedgerCreateEndpoint();
        // TODO: Implement GeneralLedger Delete endpoint
        // group.MapGeneralLedgerDeleteEndpoint();
        group.MapGeneralLedgerGetEndpoint();
        group.MapGeneralLedgerSearchEndpoint();
        group.MapGeneralLedgerUpdateEndpoint();
    }
}

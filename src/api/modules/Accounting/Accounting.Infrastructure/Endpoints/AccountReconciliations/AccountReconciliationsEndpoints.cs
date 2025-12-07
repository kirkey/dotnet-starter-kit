using Accounting.Infrastructure.Endpoints.AccountReconciliations.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliations;

/// <summary>
/// Endpoint configuration for AccountReconciliations module.
/// Provides comprehensive REST API endpoints for managing account-reconciliations.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class AccountReconciliationsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all AccountReconciliations endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/account-reconciliations").WithTags("account-reconciliation");

        group.MapApproveAccountReconciliationEndpoint();
        group.MapCreateAccountReconciliationEndpoint();
        group.MapDeleteAccountReconciliationEndpoint();
        group.MapGetAccountReconciliationEndpoint();
        group.MapReconcileGeneralLedgerAccountEndpoint();
        group.MapSearchAccountReconciliationsEndpoint();
        group.MapUpdateAccountReconciliationEndpoint();
    }
}

using Carter;
using Accounting.Infrastructure.Endpoints.AccountReconciliations.v1;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliations;

/// <summary>
/// Endpoint configuration for Account Reconciliations module.
/// </summary>
public class AccountReconciliationEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all Account Reconciliation endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/account-reconciliations").WithTags("account-reconciliations");

        group.MapCreateAccountReconciliationEndpoint();
        group.MapDeleteAccountReconciliationEndpoint();
        group.MapGetAccountReconciliationEndpoint();
        group.MapSearchAccountReconciliationsEndpoint();
        group.MapUpdateAccountReconciliationEndpoint();
        group.MapApproveAccountReconciliationEndpoint();
        group.MapReconcileGeneralLedgerAccountEndpoint();
    }
}

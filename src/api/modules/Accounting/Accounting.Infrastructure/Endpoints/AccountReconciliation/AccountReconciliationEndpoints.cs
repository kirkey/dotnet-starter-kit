using Accounting.Infrastructure.Endpoints.AccountReconciliation.v1;

namespace Accounting.Infrastructure.Endpoints.AccountReconciliation;

/// <summary>
/// Endpoint configuration for Account Reconciliation module.
/// </summary>
public static class AccountReconciliationEndpoints
{
    /// <summary>
    /// Maps all Account Reconciliation endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapAccountReconciliationEndpoints(this IEndpointRouteBuilder app)
    {
        var reconciliationGroup = app.MapGroup("/account-reconciliation")
            .WithTags("Account-Reconciliation")
            .WithDescription("Endpoints for managing account reconciliation processes");

        // Version 1 endpoints
        reconciliationGroup.MapReconcileAccountEndpoint();

        return app;
    }
}

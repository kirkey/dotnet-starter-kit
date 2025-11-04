using Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations;

/// <summary>
/// Endpoint configuration for Bank Reconciliations module.
/// </summary>
public static class BankReconciliationsEndpoints
{
    /// <summary>
    /// Maps all Bank Reconciliations endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapBankReconciliationsEndpoints(this IEndpointRouteBuilder app)
    {
        var bankReconciliationsGroup = app.MapGroup("/bank-reconciliations")
            .WithTags("Bank-Reconciliations")
            .WithDescription("Endpoints for managing bank reconciliations");

        // Version 1 endpoints
        bankReconciliationsGroup.MapCreateBankReconciliationEndpoint();
        bankReconciliationsGroup.MapUpdateBankReconciliationEndpoint();
        bankReconciliationsGroup.MapGetBankReconciliationEndpoint();
        bankReconciliationsGroup.MapDeleteBankReconciliationEndpoint();
        bankReconciliationsGroup.MapSearchBankReconciliationsEndpoint();
        bankReconciliationsGroup.MapStartBankReconciliationEndpoint();
        bankReconciliationsGroup.MapCompleteBankReconciliationEndpoint();
        bankReconciliationsGroup.MapApproveBankReconciliationEndpoint();
        bankReconciliationsGroup.MapRejectBankReconciliationEndpoint();

        return app;
    }
}

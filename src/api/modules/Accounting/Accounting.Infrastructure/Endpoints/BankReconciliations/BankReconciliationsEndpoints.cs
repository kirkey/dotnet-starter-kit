using Carter;
using Accounting.Infrastructure.Endpoints.BankReconciliations.v1;

namespace Accounting.Infrastructure.Endpoints.BankReconciliations;

/// <summary>
/// Endpoint configuration for BankReconciliations module.
/// Provides comprehensive REST API endpoints for managing bank-reconciliations.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class BankReconciliationsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all BankReconciliations endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/bank-reconciliations").WithTags("bank-reconciliation");

        group.MapApproveBankReconciliationEndpoint();
        group.MapCompleteBankReconciliationEndpoint();
        group.MapCreateBankReconciliationEndpoint();
        group.MapDeleteBankReconciliationEndpoint();
        group.MapGetBankReconciliationEndpoint();
        group.MapRejectBankReconciliationEndpoint();
        group.MapSearchBankReconciliationsEndpoint();
        group.MapStartBankReconciliationEndpoint();
        group.MapUpdateBankReconciliationEndpoint();
    }
}

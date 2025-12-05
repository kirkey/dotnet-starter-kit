using Carter;
using Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts;

/// <summary>
/// Endpoint configuration for AccountsPayableAccounts module.
/// Provides comprehensive REST API endpoints for managing accounts-payable-accounts.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class AccountsPayableAccountsEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all AccountsPayableAccounts endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/accounts-payable-accounts").WithTags("accounts-payable-account");

        group.MapApAccountCreateEndpoint();
        group.MapApAccountDeleteEndpoint();
        group.MapApAccountGetEndpoint();
        group.MapApAccountReconcileEndpoint();
        group.MapApAccountRecordDiscountLostEndpoint();
        group.MapApAccountRecordPaymentEndpoint();
        group.MapApAccountSearchEndpoint();
        group.MapApAccountUpdateBalanceEndpoint();
        group.MapApAccountUpdateEndpoint();
    }
}

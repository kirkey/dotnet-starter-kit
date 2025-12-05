using Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts;

/// <summary>
/// Endpoint configuration for AccountsPayableAccounts module.
/// Provides comprehensive REST API endpoints for managing accounts-payable-accounts.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class AccountsPayableAccountsEndpoints : ICarterModule
{
    /// <summary>
    /// Maps all AccountsPayableAccounts endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/accounts-payable-accounts").WithTags("accounts-payable-account");

        group.MapAPAccountCreateEndpoint();
        group.MapAPAccountDeleteEndpoint();
        group.MapAPAccountGetEndpoint();
        group.MapAPAccountReconcileEndpoint();
        group.MapAPAccountRecordDiscountLostEndpoint();
        group.MapAPAccountRecordPaymentEndpoint();
        group.MapAPAccountSearchEndpoint();
        group.MapAPAccountUpdateBalanceEndpoint();
        group.MapAPAccountUpdateEndpoint();
    }
}

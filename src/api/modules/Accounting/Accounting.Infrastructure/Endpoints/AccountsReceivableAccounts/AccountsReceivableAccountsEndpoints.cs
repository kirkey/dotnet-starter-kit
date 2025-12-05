using Carter;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts;

/// <summary>
/// Endpoint configuration for AccountsReceivableAccounts module.
/// Provides comprehensive REST API endpoints for managing accounts-receivable-accounts.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class AccountsReceivableAccountsEndpoints() : CarterModule("accounting")
{
    /// <summary>
    /// Maps all AccountsReceivableAccounts endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/accounts-receivable-accounts").WithTags("accounts-receivable-account");

        group.MapARAccountCreateEndpoint();
        group.MapARAccountGetEndpoint();
        group.MapARAccountReconcileEndpoint();
        group.MapARAccountRecordCollectionEndpoint();
        group.MapARAccountRecordWriteOffEndpoint();
        group.MapARAccountSearchEndpoint();
        group.MapARAccountUpdateAllowanceEndpoint();
        group.MapARAccountUpdateBalanceEndpoint();
    }
}

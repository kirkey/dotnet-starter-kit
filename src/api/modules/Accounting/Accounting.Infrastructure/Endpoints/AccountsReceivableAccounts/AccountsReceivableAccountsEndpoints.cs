using Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsReceivableAccounts;

public static class AccountsReceivableAccountsEndpoints
{
    internal static IEndpointRouteBuilder MapAccountsReceivableAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/accounts-receivable")
            .WithTags("Accounts Receivable")
            .WithDescription("Endpoints for managing AR accounts")
            .MapToApiVersion(1);

        // CRUD operations
        group.MapARAccountCreateEndpoint();
        group.MapARAccountGetEndpoint();
        group.MapARAccountSearchEndpoint();

        // Workflow operations
        group.MapARAccountUpdateBalanceEndpoint();
        group.MapARAccountUpdateAllowanceEndpoint();
        group.MapARAccountRecordWriteOffEndpoint();
        group.MapARAccountRecordCollectionEndpoint();
        group.MapARAccountReconcileEndpoint();

        return app;
    }
}


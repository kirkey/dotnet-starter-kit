using Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts;

public static class AccountsPayableAccountsEndpoints
{
    internal static IEndpointRouteBuilder MapAccountsPayableAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/accounts-payable")
            .WithTags("Accounts Payable")
            .WithDescription("Endpoints for managing AP accounts")
            .MapToApiVersion(1);

        // CRUD operations
        group.MapApAccountCreateEndpoint();
        group.MapApAccountGetEndpoint();
        group.MapApAccountSearchEndpoint();

        // Workflow operations
        group.MapApAccountUpdateBalanceEndpoint();
        group.MapApAccountRecordPaymentEndpoint();
        group.MapApAccountRecordDiscountLostEndpoint();
        group.MapApAccountReconcileEndpoint();

        return app;
    }
}

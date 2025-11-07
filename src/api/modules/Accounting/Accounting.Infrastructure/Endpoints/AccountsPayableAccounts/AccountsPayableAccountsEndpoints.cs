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
        group.MapAPAccountCreateEndpoint();
        group.MapAPAccountGetEndpoint();
        group.MapAPAccountSearchEndpoint();

        // Workflow operations
        group.MapAPAccountUpdateBalanceEndpoint();
        group.MapAPAccountRecordPaymentEndpoint();
        group.MapAPAccountRecordDiscountLostEndpoint();
        group.MapAPAccountReconcileEndpoint();

        return app;
    }
}


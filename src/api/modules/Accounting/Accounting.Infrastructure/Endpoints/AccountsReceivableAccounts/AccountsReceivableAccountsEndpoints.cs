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

        group.MapARAccountCreateEndpoint();
        group.MapARAccountGetEndpoint();
        group.MapARAccountSearchEndpoint();

        return app;
    }
}


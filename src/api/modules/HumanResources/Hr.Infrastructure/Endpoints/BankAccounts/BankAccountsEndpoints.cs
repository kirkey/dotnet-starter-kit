using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BankAccounts.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BankAccounts;

/// <summary>
/// Endpoint routes for managing bank accounts.
/// </summary>
public static class BankAccountsEndpoints
{
    internal static IEndpointRouteBuilder MapBankAccountsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/bank-accounts")
            .WithTags("Bank Accounts")
            .WithDescription("Endpoints for managing employee bank accounts for direct deposit");

        group.MapCreateBankAccountEndpoint();
        group.MapGetBankAccountEndpoint();
        group.MapUpdateBankAccountEndpoint();
        group.MapDeleteBankAccountEndpoint();
        group.MapSearchBankAccountsEndpoint();

        return app;
    }
}


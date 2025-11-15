using FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Search.v1;
using Resp = FSH.Starter.WebApi.HumanResources.Application.BankAccounts.Get.v1.BankAccountResponse;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BankAccounts.v1;

/// <summary>
/// Endpoint for searching bank accounts with filtering and pagination.
/// </summary>
public static class SearchBankAccountsEndpoint
{
    internal static RouteHandlerBuilder MapSearchBankAccountsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchBankAccountsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBankAccountsEndpoint))
            .WithSummary("Searches bank accounts")
            .WithDescription("Searches and filters bank accounts by employee, bank name, account type with pagination support. Account numbers are masked for security.")
            .Produces<PagedList<Resp>>()
            .RequirePermission("Permissions.BankAccounts.View")
            .MapToApiVersion(1);
    }
}


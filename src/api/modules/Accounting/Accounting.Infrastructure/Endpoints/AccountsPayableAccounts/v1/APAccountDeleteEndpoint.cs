using Accounting.Application.AccountsPayableAccounts.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

/// <summary>
/// Endpoint for deleting an accounts payable account.
/// </summary>
public static class ApAccountDeleteEndpoint
{
    /// <summary>
    /// Maps the AP account delete endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapApAccountDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new AccountsPayableAccountDeleteCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(ApAccountDeleteEndpoint))
            .WithSummary("Delete AP account")
            .WithDescription("Deletes an accounts payable account (only if balance is zero)")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


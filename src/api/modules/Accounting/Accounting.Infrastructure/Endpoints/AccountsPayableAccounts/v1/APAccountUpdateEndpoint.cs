using Accounting.Application.AccountsPayableAccounts.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

/// <summary>
/// Endpoint for updating an accounts payable account.
/// </summary>
public static class ApAccountUpdateEndpoint
{
    /// <summary>
    /// Maps the AP account update endpoint.
    /// </summary>
    internal static RouteHandlerBuilder MapApAccountUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, AccountsPayableAccountUpdateCommand request, ISender mediator) =>
            {
                request.GetType().GetProperty("Id")?.SetValue(request, id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApAccountUpdateEndpoint))
            .WithSummary("Update AP account")
            .WithDescription("Updates an existing accounts payable account")
            .Produces<AccountsPayableAccountUpdateResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


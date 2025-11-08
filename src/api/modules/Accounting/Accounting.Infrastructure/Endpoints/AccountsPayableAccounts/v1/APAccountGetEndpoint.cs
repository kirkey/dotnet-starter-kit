using Accounting.Application.AccountsPayableAccounts.Get;
using Accounting.Application.AccountsPayableAccounts.Responses;

namespace Accounting.Infrastructure.Endpoints.AccountsPayableAccounts.v1;

public static class ApAccountGetEndpoint
{
    internal static RouteHandlerBuilder MapApAccountGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAPAccountRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ApAccountGetEndpoint))
            .WithSummary("Get AP account by ID")
            .Produces<APAccountResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


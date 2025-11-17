using Accounting.Application.Accruals.Reverse;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualReverseEndpoint
{
    internal static RouteHandlerBuilder MapAccrualReverseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/reverse", async (DefaultIdType id, ReverseAccrualCommand command, ISender mediator) =>
            {
                command.Id = id;
                await mediator.Send(command).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(AccrualReverseEndpoint))
            .WithSummary("Reverse an accrual")
            .WithDescription("Reverses an accrual entry by ID")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Void, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

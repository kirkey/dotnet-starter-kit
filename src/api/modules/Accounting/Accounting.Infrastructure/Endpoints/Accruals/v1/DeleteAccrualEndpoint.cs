using Accounting.Application.Accruals.Delete;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class DeleteAccrualEndpoint
{
    internal static RouteHandlerBuilder MapDeleteAccrualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            await mediator.Send(new DeleteAccrualCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(DeleteAccrualEndpoint))
        .WithSummary("Delete accrual by id")
        .WithDescription("Deletes an accrual entry by its identifier")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
        .MapToApiVersion(1);
    }
}

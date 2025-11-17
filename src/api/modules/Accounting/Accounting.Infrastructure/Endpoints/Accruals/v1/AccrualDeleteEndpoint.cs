// Endpoint for deleting an accrual

using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

using Accounting.Application.Accruals.Delete;

/// <summary>
/// Endpoints for deleting Accruals.
/// </summary>
public static class AccrualDeleteEndpoint
{
    /// <summary>
    /// Maps DELETE /{id:guid} for Accruals.
    /// </summary>
    internal static RouteHandlerBuilder MapAccrualDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteAccrualCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(AccrualDeleteEndpoint))
            .WithSummary("Delete accrual by id")
            .WithDescription("Deletes an accrual entry by its identifier")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

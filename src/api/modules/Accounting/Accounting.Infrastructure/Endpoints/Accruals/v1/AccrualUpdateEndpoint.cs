using Accounting.Application.Accruals.Update;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

/// <summary>
/// Endpoints for editing Accruals.
/// </summary>
public static class AccrualUpdateEndpoint
{
    /// <summary>
    /// Maps PUT /{id:guid} to update mutable fields of an accrual.
    /// </summary>
    internal static RouteHandlerBuilder MapAccrualUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateAccrualCommand request, ISender mediator) =>
            {
                request.Id = id; // Directly set the ID
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccrualUpdateEndpoint))
            .WithSummary("Update an accrual")
            .WithDescription("Updates an accrual's mutable fields")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

using Accounting.Application.Accruals.Update;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

/// <summary>
/// Endpoints for editing Accruals.
/// </summary>
public static class AccrualEditEndpoint
{
    /// <summary>
    /// Maps PUT /{id:guid} to update mutable fields of an accrual.
    /// </summary>
    internal static RouteHandlerBuilder MapAccrualEditEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateAccrualCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccrualEditEndpoint))
            .WithSummary("Update an accrual")
            .WithDescription("Updates an accrual's mutable fields")
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(1);
    }
}

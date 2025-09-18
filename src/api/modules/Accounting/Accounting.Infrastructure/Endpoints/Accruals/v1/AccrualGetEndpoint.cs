using Accounting.Application.Accruals.Dtos;
using Accounting.Application.Accruals.Queries;

namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualGetEndpoint
{
    internal static RouteHandlerBuilder MapAccrualGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAccrualByIdQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccrualGetEndpoint))
            .WithSummary("Get an accrual by ID")
            .WithDescription("Gets the details of an accrual by its ID")
            .Produces<AccrualDto>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

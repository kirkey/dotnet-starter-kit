using FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts.v1;

public static class GetShiftEndpoint
{
    internal static RouteHandlerBuilder MapGetShiftEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetShiftRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetShiftEndpoint))
            .WithSummary("Gets shift by ID")
            .WithDescription("Retrieves shift details with breaks and working hours")
            .Produces<ShiftResponse>()
            .RequirePermission("Permissions.Employees.View")
            .MapToApiVersion(1);
    }
}


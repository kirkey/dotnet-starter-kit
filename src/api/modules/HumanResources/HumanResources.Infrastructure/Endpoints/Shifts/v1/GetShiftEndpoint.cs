using FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts.v1;

public static class GetShiftEndpoint
{
    internal static RouteHandlerBuilder MapGetShiftEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetShiftRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetShiftEndpoint))
            .WithSummary("Gets shift by ID")
            .WithDescription("Retrieves shift details with breaks and working hours")
            .Produces<ShiftResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}


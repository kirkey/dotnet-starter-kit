using FSH.Starter.WebApi.HumanResources.Application.Shifts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Shifts.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts.v1;

public static class SearchShiftsEndpoint
{
    internal static RouteHandlerBuilder MapSearchShiftsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchShiftsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchShiftsEndpoint))
            .WithSummary("Searches shifts")
            .WithDescription("Searches shifts with pagination and filters")
            .Produces<PagedList<ShiftResponse>>()
            .RequirePermission("Permissions.Employees.View")
            .MapToApiVersion(1);
    }
}


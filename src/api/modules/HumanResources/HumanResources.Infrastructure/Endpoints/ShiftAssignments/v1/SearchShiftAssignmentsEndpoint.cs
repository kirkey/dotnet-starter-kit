using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments.v1;

public static class SearchShiftAssignmentsEndpoint
{
    internal static RouteHandlerBuilder MapSearchShiftAssignmentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchShiftAssignmentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchShiftAssignmentsEndpoint))
            .WithSummary("Searches shift assignments")
            .WithDescription("Searches and filters shift assignments with pagination")
            .Produces<PagedList<ShiftAssignmentResponse>>()
            .RequirePermission("Permissions.ShiftAssignments.View")
            .MapToApiVersion(1);
    }
}


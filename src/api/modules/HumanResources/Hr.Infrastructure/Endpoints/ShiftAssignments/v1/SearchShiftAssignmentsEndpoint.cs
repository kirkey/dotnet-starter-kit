using FSH.Starter.WebApi.HumanResources.Application.ShiftAssignments.Get.v1;
using Shared.Authorization;
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
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
            .MapToApiVersion(1);
    }
}


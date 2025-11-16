using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveTypes.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes.v1;

public static class SearchLeaveTypesEndpoint
{
    internal static RouteHandlerBuilder MapSearchLeaveTypesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchLeaveTypesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchLeaveTypesEndpoint))
            .WithSummary("Searches leave types")
            .WithDescription("Searches and filters leave types with pagination support")
            .Produces<PagedList<LeaveTypeResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Leaves))
            .MapToApiVersion(1);
    }
}


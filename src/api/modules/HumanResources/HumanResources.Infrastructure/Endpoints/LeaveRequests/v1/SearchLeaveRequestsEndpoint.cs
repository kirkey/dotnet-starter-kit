using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.LeaveRequests.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveRequests.v1;

/// <summary>
/// Endpoint for searching leave requests with filtering and pagination.
/// </summary>
public static class SearchLeaveRequestsEndpoint
{
    internal static RouteHandlerBuilder MapSearchLeaveRequestsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchLeaveRequestsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchLeaveRequestsEndpoint))
            .WithSummary("Searches leave requests")
            .WithDescription("Searches and filters leave requests by employee, leave type, status, and date range with pagination support. Supports advanced filtering per Philippines Labor Code compliance requirements.")
            .Produces<PagedList<LeaveRequestResponse>>()
            .RequirePermission("Permissions.LeaveRequests.View")
            .MapToApiVersion(1);
    }
}


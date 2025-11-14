using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.HumanResources.Application.Attendance.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Attendance.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance.v1;

public static class SearchAttendanceEndpoint
{
    internal static RouteHandlerBuilder MapSearchAttendanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchAttendanceRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchAttendanceEndpoint))
            .WithSummary("Searches attendance records")
            .WithDescription("Searches attendance records with pagination and filters")
            .Produces<PagedList<AttendanceResponse>>()
            .RequirePermission("Permissions.Attendance.View")
            .MapToApiVersion(1);
    }
}


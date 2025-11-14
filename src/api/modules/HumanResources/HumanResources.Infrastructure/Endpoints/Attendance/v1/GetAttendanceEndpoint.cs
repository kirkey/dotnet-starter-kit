using FSH.Starter.WebApi.HumanResources.Application.Attendances.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance.v1;

public static class GetAttendanceEndpoint
{
    internal static RouteHandlerBuilder MapGetAttendanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAttendanceRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetAttendanceEndpoint))
            .WithSummary("Gets attendance record by ID")
            .WithDescription("Retrieves attendance details")
            .Produces<AttendanceResponse>()
            .RequirePermission("Permissions.Attendance.View")
            .MapToApiVersion(1);
    }
}


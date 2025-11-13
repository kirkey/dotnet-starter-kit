using FSH.Starter.WebApi.HumanResources.Application.Attendance.Get.v1;

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
            .WithDescription("Retrieves attendance record details")
            .Produces<AttendanceResponse>()
            .RequirePermission("Permissions.Employees.View")
            .MapToApiVersion(1);
    }
}


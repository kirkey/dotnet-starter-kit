using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance;

public static class AttendanceEndpoints
{
    internal static IEndpointRouteBuilder MapAttendanceEndpoints(this IEndpointRouteBuilder app)
    {
        var attendanceGroup = app.MapGroup("/attendance")
            .WithTags("Attendance")
            .WithDescription("Endpoints for managing employee attendance (clock in/out, daily records)");

        attendanceGroup.MapCreateAttendanceEndpoint();
        attendanceGroup.MapGetAttendanceEndpoint();
        attendanceGroup.MapSearchAttendanceEndpoint();
        attendanceGroup.MapUpdateAttendanceEndpoint();
        attendanceGroup.MapDeleteAttendanceEndpoint();

        return app;
    }
}


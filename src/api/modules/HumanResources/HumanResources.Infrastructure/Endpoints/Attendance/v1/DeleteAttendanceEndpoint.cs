using FSH.Starter.WebApi.HumanResources.Application.Attendances.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance.v1;

public static class DeleteAttendanceEndpoint
{
    internal static RouteHandlerBuilder MapDeleteAttendanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteAttendanceCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteAttendanceEndpoint))
            .WithSummary("Deletes an attendance record")
            .WithDescription("Deletes an attendance record")
            .Produces<DeleteAttendanceResponse>()
            .RequirePermission("Permissions.Attendance.Delete")
            .MapToApiVersion(1);
    }
}


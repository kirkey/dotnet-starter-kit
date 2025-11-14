using FSH.Starter.WebApi.HumanResources.Application.Attendance.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance.v1;

public static class UpdateAttendanceEndpoint
{
    internal static RouteHandlerBuilder MapUpdateAttendanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateAttendanceCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateAttendanceEndpoint))
            .WithSummary("Updates an attendance record")
            .WithDescription("Updates attendance information")
            .Produces<UpdateAttendanceResponse>()
            .RequirePermission("Permissions.Attendance.Edit")
            .MapToApiVersion(1);
    }
}


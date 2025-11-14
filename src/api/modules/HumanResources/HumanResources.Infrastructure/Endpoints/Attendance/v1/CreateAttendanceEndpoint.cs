using FSH.Starter.WebApi.HumanResources.Application.Attendance.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance.v1;

public static class CreateAttendanceEndpoint
{
    internal static RouteHandlerBuilder MapCreateAttendanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAttendanceCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetAttendanceEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateAttendanceEndpoint))
            .WithSummary("Creates a new attendance record")
            .WithDescription("Records employee attendance for a specific date")
            .Produces<CreateAttendanceResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.Attendance.Create")
            .MapToApiVersion(1);
    }
}


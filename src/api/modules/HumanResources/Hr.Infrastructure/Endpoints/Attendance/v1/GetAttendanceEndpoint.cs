using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Attendances.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Attendance.v1;

public static class GetAttendanceEndpoint
{
    internal static RouteHandlerBuilder MapGetAttendanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAttendanceRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetAttendanceEndpoint))
            .WithSummary("Gets attendance record by ID")
            .WithDescription("Retrieves attendance details")
            .Produces<AttendanceResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Attendance))
            .MapToApiVersion(1);
    }
}


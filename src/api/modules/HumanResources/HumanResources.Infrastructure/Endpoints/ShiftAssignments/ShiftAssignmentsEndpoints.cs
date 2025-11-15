using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.ShiftAssignments;
public static class ShiftAssignmentsEndpoints
{
    internal static IEndpointRouteBuilder MapShiftAssignmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/shift-assignments")
            .WithTags("Shift Assignments")
            .WithDescription("Endpoints for managing employee shift assignments");
        group.MapCreateShiftAssignmentEndpoint();
        group.MapGetShiftAssignmentEndpoint();
        group.MapUpdateShiftAssignmentEndpoint();
        group.MapDeleteShiftAssignmentEndpoint();
        group.MapSearchShiftAssignmentsEndpoint();
        return app;
    }
}

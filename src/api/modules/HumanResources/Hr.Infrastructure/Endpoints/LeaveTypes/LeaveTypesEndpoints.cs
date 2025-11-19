using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.LeaveTypes;

public static class LeaveTypesEndpoints
{
    internal static IEndpointRouteBuilder MapLeaveTypesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/leave-types")
            .WithTags("Leave Types")
            .WithDescription("Endpoints for managing leave types with Philippines Labor Code compliance");

        group.MapCreateLeaveTypeEndpoint();
        group.MapGetLeaveTypeEndpoint();
        group.MapUpdateLeaveTypeEndpoint();
        group.MapDeleteLeaveTypeEndpoint();
        group.MapSearchLeaveTypesEndpoint();

        return app;
    }
}


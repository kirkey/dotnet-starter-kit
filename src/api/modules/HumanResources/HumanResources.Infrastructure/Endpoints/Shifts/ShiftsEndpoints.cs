using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Shifts;

public static class ShiftsEndpoints
{
    internal static IEndpointRouteBuilder MapShiftsEndpoints(this IEndpointRouteBuilder app)
    {
        var shiftsGroup = app.MapGroup("/shifts")
            .WithTags("Shifts")
            .WithDescription("Endpoints for managing shifts");

        shiftsGroup.MapCreateShiftEndpoint();
        shiftsGroup.MapGetShiftEndpoint();
        shiftsGroup.MapSearchShiftsEndpoint();
        shiftsGroup.MapUpdateShiftEndpoint();
        shiftsGroup.MapDeleteShiftEndpoint();

        return app;
    }
}


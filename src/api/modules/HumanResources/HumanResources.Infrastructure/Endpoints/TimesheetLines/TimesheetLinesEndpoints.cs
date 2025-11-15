using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines;

/// <summary>
/// Endpoint configuration for TimesheetLines module.
/// </summary>
public static class TimesheetLinesEndpoints
{
    /// <summary>
    /// Maps all TimesheetLines endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapTimesheetLinesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/timesheet-lines")
            .WithTags("Timesheet Lines")
            .WithDescription("Endpoints for managing timesheet daily entries");

        group.MapCreateTimesheetLineEndpoint();
        group.MapGetTimesheetLineEndpoint();
        group.MapSearchTimesheetLinesEndpoint();
        group.MapUpdateTimesheetLineEndpoint();
        group.MapDeleteTimesheetLineEndpoint();

        return app;
    }
}


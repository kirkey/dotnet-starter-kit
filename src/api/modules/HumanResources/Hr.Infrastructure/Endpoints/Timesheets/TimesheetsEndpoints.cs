using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets;

public static class TimesheetsEndpoints
{
    internal static IEndpointRouteBuilder MapTimesheetsEndpoints(this IEndpointRouteBuilder app)
    {
        var timesheetsGroup = app.MapGroup("/timesheets")
            .WithTags("Timesheets")
            .WithDescription("Endpoints for managing employee timesheets");

        timesheetsGroup.MapCreateTimesheetEndpoint();
        timesheetsGroup.MapGetTimesheetEndpoint();
        timesheetsGroup.MapSearchTimesheetsEndpoint();
        timesheetsGroup.MapUpdateTimesheetEndpoint();
        timesheetsGroup.MapDeleteTimesheetEndpoint();

        return app;
    }
}


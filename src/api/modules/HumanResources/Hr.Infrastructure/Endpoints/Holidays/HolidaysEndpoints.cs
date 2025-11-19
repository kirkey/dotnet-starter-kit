using FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays;

public static class HolidaysEndpoints
{
    internal static IEndpointRouteBuilder MapHolidaysEndpoints(this IEndpointRouteBuilder app)
    {
        var holidaysGroup = app.MapGroup("/holidays")
            .WithTags("Holidays")
            .WithDescription("Endpoints for managing holidays");

        holidaysGroup.MapCreateHolidayEndpoint();
        holidaysGroup.MapGetHolidayEndpoint();
        holidaysGroup.MapSearchHolidaysEndpoint();
        holidaysGroup.MapUpdateHolidayEndpoint();
        holidaysGroup.MapDeleteHolidayEndpoint();

        return app;
    }
}


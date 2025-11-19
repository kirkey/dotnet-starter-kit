namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates;

using v1;
using Microsoft.AspNetCore.Routing;

public static class PayComponentRateEndpoints
{
    internal static void MapPayComponentRatesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/paycomponent-rates")
            .WithTags("PayComponentRates")
            .WithGroupName("Payroll Configuration");

        CreatePayComponentRateEndpoint.MapCreatePayComponentRateEndpoint(group);
        UpdatePayComponentRateEndpoint.MapUpdatePayComponentRateEndpoint(group);
        GetPayComponentRateEndpoint.MapGetPayComponentRateEndpoint(group);
        DeletePayComponentRateEndpoint.MapDeletePayComponentRateEndpoint(group);
        SearchPayComponentRatesEndpoint.MapSearchPayComponentRatesEndpoint(group);
    }
}


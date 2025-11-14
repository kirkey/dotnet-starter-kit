namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates;

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
    }
}


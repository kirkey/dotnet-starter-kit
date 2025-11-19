namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents;

using v1;

public static class PayComponentEndpoints
{
    internal static void MapPayComponentsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/paycomponents")
            .WithTags("PayComponents")
            .WithGroupName("Payroll Configuration");

        group.MapCreatePayComponentEndpoint();
        group.MapUpdatePayComponentEndpoint();
        group.MapGetPayComponentEndpoint();
        group.MapDeletePayComponentEndpoint();
        group.MapSearchPayComponentsEndpoint();
    }
}


namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Benefits;

using v1;

public static class BenefitEndpoints
{
    internal static void MapBenefitEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/benefits")
            .WithTags("Benefits")
            .WithGroupName("Benefits Catalog")
            .WithDescription("Endpoints for managing benefit master data (mandatory & optional benefit offerings)");

        group.MapCreateBenefitEndpoint();
        group.MapGetBenefitEndpoint();
        group.MapSearchBenefitsEndpoint();
        group.MapUpdateBenefitEndpoint();
        group.MapDeleteBenefitEndpoint();
    }
}



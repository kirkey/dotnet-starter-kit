namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TaxBrackets;

using v1;

public static class TaxBracketEndpoints
{
    public static void MapTaxBracketEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("tax-brackets").WithTags("TaxBrackets");
        group.MapCreateTaxBracketEndpoint();
        group.MapUpdateTaxBracketEndpoint();
        group.MapGetTaxBracketEndpoint();
        group.MapDeleteTaxBracketEndpoint();
        group.MapSearchTaxBracketsEndpoint();
    }
}



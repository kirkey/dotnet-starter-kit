using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Search.v1;
using Resp = FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Get.v1.TaxBracketResponse;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TaxBrackets.v1;

/// <summary>
/// Endpoint for searching tax brackets with filtering and pagination.
/// </summary>
public static class SearchTaxBracketsEndpoint
{
    internal static RouteHandlerBuilder MapSearchTaxBracketsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchTaxBracketsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchTaxBracketsEndpoint))
            .WithSummary("Searches tax brackets")
            .WithDescription("Searches and filters tax brackets by type, year, filing status, and income range with pagination support.")
            .Produces<PagedList<Resp>>()
            .RequirePermission("Permissions.TaxBrackets.View")
            .MapToApiVersion(1);
    }
}


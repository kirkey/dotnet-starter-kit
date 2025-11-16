using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Get.v1;
using Shared.Authorization;

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
            .Produces<PagedList<TaxBracketResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}


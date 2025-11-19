using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Search.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponentRates.v1;

/// <summary>
/// Endpoint for searching pay component rates with filtering and pagination.
/// </summary>
public static class SearchPayComponentRatesEndpoint
{
    internal static RouteHandlerBuilder MapSearchPayComponentRatesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPayComponentRatesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPayComponentRatesEndpoint))
            .WithSummary("Searches pay component rates")
            .WithDescription("Searches and filters pay component rates (tax brackets, SSS rates, etc.) by component, year, amount range with pagination support.")
            .Produces<PagedList<PayComponentRateResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}


using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayComponents.v1;

/// <summary>
/// Endpoint for searching pay components with filtering and pagination.
/// </summary>
public static class SearchPayComponentsEndpoint
{
    internal static RouteHandlerBuilder MapSearchPayComponentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPayComponentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPayComponentsEndpoint))
            .WithSummary("Searches pay components")
            .WithDescription("Searches and filters pay components by type, calculation method, active status with pagination support.")
            .Produces<PagedList<PayComponentResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}


using FSH.Starter.WebApi.HumanResources.Application.PayComponents.Search.v1;
using Resp = FSH.Starter.WebApi.HumanResources.Application.PayComponents.Get.v1.PayComponentResponse;

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
            .Produces<PagedList<Resp>>()
            .RequirePermission("Permissions.PayComponents.View")
            .MapToApiVersion(1);
    }
}


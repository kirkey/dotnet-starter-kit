using FSH.Starter.WebApi.HumanResources.Application.Designations.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Designations.Search.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Designations.v1;

/// <summary>
/// Endpoint for searching designations.
/// </summary>
public static class SearchDesignationsEndpoint
{
    internal static RouteHandlerBuilder MapDesignationsSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchDesignationsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchDesignationsEndpoint))
            .WithSummary("Searches designations")
            .WithDescription("Searches designations with pagination and filters")
            .Produces<PagedList<DesignationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Organization))
            .MapToApiVersion(1);
    }
}

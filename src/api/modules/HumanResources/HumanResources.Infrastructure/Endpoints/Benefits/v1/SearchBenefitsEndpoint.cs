using FSH.Starter.WebApi.HumanResources.Application.Benefits.Search.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Benefits.v1;

/// <summary>
/// Endpoint for searching benefits.
/// </summary>
public static class SearchBenefitsEndpoint
{
    internal static RouteHandlerBuilder MapSearchBenefitsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/", async (SearchBenefitsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchBenefitsEndpoint))
            .WithSummary("Search Benefits")
            .WithDescription("Search benefit catalog by type, mandatory flag, and active status with pagination.")
            .Produces<PagedList<BenefitDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


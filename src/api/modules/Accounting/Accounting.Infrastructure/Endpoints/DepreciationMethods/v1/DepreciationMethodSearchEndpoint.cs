using Accounting.Application.DepreciationMethods.Responses;
using Accounting.Application.DepreciationMethods.Search.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DepreciationMethods.v1;

public static class DepreciationMethodSearchEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationMethodSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchDepreciationMethodsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DepreciationMethodSearchEndpoint))
            .WithSummary("Search depreciation methods")
            .WithDescription("Searches depreciation methods with filtering and pagination")
            .Produces<PagedList<DepreciationMethodResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


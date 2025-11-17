using FSH.Starter.WebApi.Store.Application.LotNumbers.Search.v1;
using Shared.Authorization;
using LotNumberResponse = FSH.Starter.WebApi.Store.Application.LotNumbers.Get.v1.LotNumberResponse;

namespace Store.Infrastructure.Endpoints.LotNumbers.v1;

public static class SearchLotNumbersEndpoint
{
    internal static RouteHandlerBuilder MapSearchLotNumbersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchLotNumbersCommand command, ISender sender) =>
            {
                var response = await sender.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchLotNumbersEndpoint))
            .WithSummary("Search lot numbers")
            .WithDescription("Search and filter lot numbers with expiration tracking and pagination support")
            .Produces<PagedList<LotNumberResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Store))
            .MapToApiVersion(1);
    }
}

using FSH.Starter.WebApi.HumanResources.Application.Holidays.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.Holidays.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Holidays.v1;

public static class SearchHolidaysEndpoint
{
    internal static RouteHandlerBuilder MapSearchHolidaysEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchHolidaysRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchHolidaysEndpoint))
            .WithSummary("Searches holidays")
            .WithDescription("Searches and filters holidays with pagination support")
            .Produces<PagedList<HolidayResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Organization))
            .MapToApiVersion(1);
    }
}

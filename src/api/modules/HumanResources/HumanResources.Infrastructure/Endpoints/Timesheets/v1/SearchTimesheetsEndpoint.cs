using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Timesheets.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Timesheets.v1;

public static class SearchTimesheetsEndpoint
{
    internal static RouteHandlerBuilder MapSearchTimesheetsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchTimesheetsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchTimesheetsEndpoint))
            .WithSummary("Searches timesheets")
            .WithDescription("Searches timesheets with pagination and filters")
            .Produces<PagedList<TimesheetResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Timesheets))
            .MapToApiVersion(1);
    }
}


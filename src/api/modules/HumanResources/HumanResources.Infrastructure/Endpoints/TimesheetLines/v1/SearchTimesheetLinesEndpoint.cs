using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.TimesheetLines.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.TimesheetLines.v1;

/// <summary>
/// Endpoint for searching timesheet lines with pagination and filters.
/// </summary>
public static class SearchTimesheetLinesEndpoint
{
    internal static RouteHandlerBuilder MapSearchTimesheetLinesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchTimesheetLinesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchTimesheetLinesEndpoint))
            .WithSummary("Searches timesheet lines")
            .WithDescription("Searches and filters timesheet lines with pagination")
            .Produces<PagedList<TimesheetLineResponse>>()
            .RequirePermission("Permissions.TimesheetLines.View")
            .MapToApiVersion(1);
    }
}


namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollReports.v1;

using FSH.Starter.WebApi.HumanResources.Application.PayrollReports.Search.v1;
using Shared.Authorization;

/// <summary>
/// Endpoint for searching payroll reports.
/// </summary>
public static class SearchPayrollReportsEndpoint
{
    /// <summary>
    /// Maps the search payroll reports endpoint.
    /// </summary>
    public static RouteHandlerBuilder MapSearchPayrollReportsEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/search", async (SearchPayrollReportsRequest request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SearchPayrollReportsEndpoint))
        .WithSummary("Search payroll reports")
        .WithDescription("Searches and filters payroll reports with pagination support")
        .Produces<PagedList<PayrollReportDto>>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}


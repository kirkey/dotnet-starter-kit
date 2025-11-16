using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Search.v1;
using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollLines.v1;

/// <summary>
/// Endpoint for searching payroll lines with filtering and pagination.
/// </summary>
public static class SearchPayrollLinesEndpoint
{
    internal static RouteHandlerBuilder MapSearchPayrollLinesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPayrollLinesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPayrollLinesEndpoint))
            .WithSummary("Searches payroll lines")
            .WithDescription("Searches and filters payroll lines by payroll period, employee, and other criteria with pagination support.")
            .Produces<PagedList<PayrollLineResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}


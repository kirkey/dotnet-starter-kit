using FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Search.v1;
using Resp = FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Get.v1.PayrollLineResponse;

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
            .Produces<PagedList<Resp>>()
            .RequirePermission("Permissions.PayrollLines.View")
            .MapToApiVersion(1);
    }
}


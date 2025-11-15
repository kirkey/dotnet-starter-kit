using FSH.Starter.WebApi.HumanResources.Application.Payrolls.Search.v1;
using Resp = FSH.Starter.WebApi.HumanResources.Application.Payrolls.Get.v1.PayrollResponse;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Payrolls.v1;

/// <summary>
/// Endpoint for searching payroll periods with filtering and pagination.
/// </summary>
public static class SearchPayrollsEndpoint
{
    internal static RouteHandlerBuilder MapSearchPayrollsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPayrollsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPayrollsEndpoint))
            .WithSummary("Searches payroll periods")
            .WithDescription("Searches and filters payroll periods by date range, status, and pay frequency with pagination support.")
            .Produces<PagedList<Resp>>()
            .RequirePermission("Permissions.Payrolls.View")
            .MapToApiVersion(1);
    }
}


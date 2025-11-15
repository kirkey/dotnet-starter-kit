using FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Search.v1;
using Resp = FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Get.v1.PayrollDeductionResponse;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.PayrollDeductions.v1;

/// <summary>
/// Endpoint for searching payroll deductions with filtering and pagination.
/// </summary>
public static class SearchPayrollDeductionsEndpoint
{
    internal static RouteHandlerBuilder MapSearchPayrollDeductionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPayrollDeductionsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPayrollDeductionsEndpoint))
            .WithSummary("Searches payroll deductions")
            .WithDescription("Searches and filters payroll deductions by type, employee, department, authorization status with pagination support.")
            .Produces<PagedList<Resp>>()
            .RequirePermission("Permissions.PayrollDeductions.View")
            .MapToApiVersion(1);
    }
}


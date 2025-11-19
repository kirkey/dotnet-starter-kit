using FSH.Starter.WebApi.HumanResources.Application.Deductions.Search.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Deductions.v1;

/// <summary>
/// Endpoint for searching deductions.
/// </summary>
public static class SearchDeductionsEndpoint
{
    internal static RouteHandlerBuilder MapSearchDeductionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchDeductionsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchDeductionsEndpoint))
            .WithSummary("Search Deductions")
            .WithDescription("Search deduction types by type, recovery method, and active status with pagination.")
            .Produces<PagedList<DeductionDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Search, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}


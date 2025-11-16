using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Search.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeEducations.v1;

/// <summary>
/// Endpoint for searching employee education records with pagination and filters.
/// </summary>
public static class SearchEmployeeEducationsEndpoint
{
    internal static RouteHandlerBuilder MapSearchEmployeeEducationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchEmployeeEducationsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchEmployeeEducationsEndpoint))
            .WithSummary("Searches employee education records")
            .WithDescription("Searches and filters employee education records with pagination")
            .Produces<PagedList<EmployeeEducationResponse>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents.v1;

/// <summary>
/// Endpoint for searching employee dependents.
/// </summary>
public static class SearchEmployeeDependentsEndpoint
{
    internal static RouteHandlerBuilder MapSearchEmployeeDependentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchEmployeeDependentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchEmployeeDependentsEndpoint))
            .WithSummary("Searches employee dependents")
            .WithDescription("Searches employee dependents with pagination and filters")
            .Produces<PagedList<EmployeeDependentResponse>>()
            .RequirePermission("Permissions.Employees.View")
            .MapToApiVersion(1);
    }
}


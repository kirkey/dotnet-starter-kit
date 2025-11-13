using FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.Employees.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees.v1;

/// <summary>
/// Endpoint for searching employees.
/// </summary>
public static class SearchEmployeesEndpoint
{
    internal static RouteHandlerBuilder MapSearchEmployeesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchEmployeesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchEmployeesEndpoint))
            .WithSummary("Searches employees")
            .WithDescription("Searches employees with pagination and filters")
            .Produces<PagedList<EmployeeResponse>>()
            .RequirePermission("Permissions.Employees.View")
            .MapToApiVersion(1);
    }
}


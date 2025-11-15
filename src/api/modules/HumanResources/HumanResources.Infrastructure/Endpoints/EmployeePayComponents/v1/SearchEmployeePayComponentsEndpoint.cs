using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Search.v1;
using Resp = FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Get.v1.EmployeePayComponentResponse;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeePayComponents.v1;

/// <summary>
/// Endpoint for searching employee pay components with filtering and pagination.
/// </summary>
public static class SearchEmployeePayComponentsEndpoint
{
    internal static RouteHandlerBuilder MapSearchEmployeePayComponentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchEmployeePayComponentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchEmployeePayComponentsEndpoint))
            .WithSummary("Searches employee pay components")
            .WithDescription("Searches and filters employee pay component assignments by employee, component, type, and active status with pagination support.")
            .Produces<PagedList<Resp>>()
            .RequirePermission("Permissions.EmployeePayComponents.View")
            .MapToApiVersion(1);
    }
}


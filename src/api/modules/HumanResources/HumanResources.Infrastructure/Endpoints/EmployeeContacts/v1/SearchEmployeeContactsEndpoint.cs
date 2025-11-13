using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts.v1;

/// <summary>
/// Endpoint for searching employee contacts.
/// </summary>
public static class SearchEmployeeContactsEndpoint
{
    internal static RouteHandlerBuilder MapSearchEmployeeContactsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchEmployeeContactsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchEmployeeContactsEndpoint))
            .WithSummary("Searches employee contacts")
            .WithDescription("Searches employee contacts with pagination and filters")
            .Produces<PagedList<EmployeeContactResponse>>()
            .RequirePermission("Permissions.Employees.View")
            .MapToApiVersion(1);
    }
}


using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Get.v1;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Search.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments.v1;

public static class SearchEmployeeDocumentsEndpoint
{
    internal static RouteHandlerBuilder MapSearchEmployeeDocumentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchEmployeeDocumentsRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchEmployeeDocumentsEndpoint))
            .WithSummary("Searches employee documents")
            .WithDescription("Searches employee documents with pagination and filters")
            .Produces<PagedList<EmployeeDocumentResponse>>()
            .RequirePermission("Permissions.Employees.View")
            .MapToApiVersion(1);
    }
}


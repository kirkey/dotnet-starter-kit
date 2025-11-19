using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments.v1;

public static class CreateEmployeeDocumentEndpoint
{
    internal static RouteHandlerBuilder MapCreateEmployeeDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateEmployeeDocumentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetEmployeeDocumentEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateEmployeeDocumentEndpoint))
            .WithSummary("Creates a new employee document")
            .WithDescription("Creates a new employee document (contract, certification, license, etc.)")
            .Produces<CreateEmployeeDocumentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}


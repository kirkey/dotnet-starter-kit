using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Delete.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments.v1;

public static class DeleteEmployeeDocumentEndpoint
{
    internal static RouteHandlerBuilder MapDeleteEmployeeDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeeDocumentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteEmployeeDocumentEndpoint))
            .WithSummary("Deletes an employee document")
            .WithDescription("Deletes an employee document record")
            .Produces<DeleteEmployeeDocumentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}


using FSH.Starter.WebApi.HumanResources.Application.EmployeeDocuments.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDocuments.v1;

public static class UpdateEmployeeDocumentEndpoint
{
    internal static RouteHandlerBuilder MapUpdateEmployeeDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateEmployeeDocumentCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateEmployeeDocumentEndpoint))
            .WithSummary("Updates an employee document")
            .WithDescription("Updates employee document information")
            .Produces<UpdateEmployeeDocumentResponse>()
            .RequirePermission("Permissions.Employees.Manage")
            .MapToApiVersion(1);
    }
}


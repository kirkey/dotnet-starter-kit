using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts.v1;

/// <summary>
/// Endpoint for deleting an employee contact.
/// </summary>
public static class DeleteEmployeeContactEndpoint
{
    internal static RouteHandlerBuilder MapDeleteEmployeeContactEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeeContactCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteEmployeeContactEndpoint))
            .WithSummary("Deletes an employee contact")
            .WithDescription("Deletes an employee contact record")
            .Produces<DeleteEmployeeContactResponse>()
            .RequirePermission("Permissions.Employees.Manage")
            .MapToApiVersion(1);
    }
}


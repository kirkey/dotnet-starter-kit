using FSH.Starter.WebApi.HumanResources.Application.Employees.Delete.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees.v1;

public static class DeleteEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapDeleteEmployeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeeCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteEmployeeEndpoint))
            .WithSummary("Deletes an employee")
            .WithDescription("Deletes an employee record")
            .Produces<DeleteEmployeeResponse>()
            .RequirePermission("Permissions.Employees.Delete")
            .MapToApiVersion(1);
    }
}


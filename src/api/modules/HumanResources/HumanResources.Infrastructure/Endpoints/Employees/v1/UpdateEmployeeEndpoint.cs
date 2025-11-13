using FSH.Starter.WebApi.HumanResources.Application.Employees.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees.v1;

/// <summary>
/// Endpoint for updating an employee.
/// </summary>
public static class UpdateEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapUpdateEmployeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateEmployeeCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateEmployeeEndpoint))
            .WithSummary("Updates an employee")
            .WithDescription("Updates employee information")
            .Produces<UpdateEmployeeResponse>()
            .RequirePermission("Permissions.Employees.Update")
            .MapToApiVersion(1);
    }
}


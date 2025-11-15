using FSH.Starter.WebApi.HumanResources.Application.Employees.Terminate.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees.v1;

/// <summary>
/// Endpoint for terminating an employee per Philippines Labor Code.
/// Computes separation pay and updates employee status to Terminated.
/// </summary>
public static class TerminateEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapTerminateEmployeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/terminate", async (DefaultIdType id, TerminateEmployeeCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(TerminateEmployeeEndpoint))
            .WithSummary("Terminates an employee")
            .WithDescription("Terminates an employee per Philippines Labor Code. Computes separation pay.")
            .Produces<TerminateEmployeeResponse>()
            .RequirePermission("Permissions.Employees.Terminate")
            .MapToApiVersion(1);
    }
}


using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Delete.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents.v1;

/// <summary>
/// Endpoint for deleting an employee dependent.
/// </summary>
public static class DeleteEmployeeDependentEndpoint
{
    internal static RouteHandlerBuilder MapDeleteEmployeeDependentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeeDependentCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteEmployeeDependentEndpoint))
            .WithSummary("Deletes an employee dependent")
            .WithDescription("Deletes an employee dependent record")
            .Produces<DeleteEmployeeDependentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}


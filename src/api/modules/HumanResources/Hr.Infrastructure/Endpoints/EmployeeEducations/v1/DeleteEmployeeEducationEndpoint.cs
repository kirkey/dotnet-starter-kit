using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Delete.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeEducations.v1;

/// <summary>
/// Endpoint for deleting an employee education record.
/// </summary>
public static class DeleteEmployeeEducationEndpoint
{
    internal static RouteHandlerBuilder MapDeleteEmployeeEducationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteEmployeeEducationCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteEmployeeEducationEndpoint))
            .WithSummary("Deletes an employee education record")
            .WithDescription("Deletes a specific employee education record from the system")
            .Produces<DeleteEmployeeEducationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

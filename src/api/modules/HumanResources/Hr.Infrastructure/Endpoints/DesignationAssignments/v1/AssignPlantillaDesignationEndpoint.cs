using FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments.v1;

/// <summary>
/// Endpoint for assigning a plantilla (primary) designation to an employee.
/// </summary>
public static class AssignPlantillaDesignationEndpoint
{
    internal static RouteHandlerBuilder MapAssignPlantillaDesignationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/plantilla", async (AssignPlantillaDesignationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AssignPlantillaDesignationEndpoint))
            .WithSummary("Assigns a plantilla designation to an employee")
            .WithDescription("Assigns a primary/plantilla designation to an employee")
            .Produces<AssignDesignationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Assign, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

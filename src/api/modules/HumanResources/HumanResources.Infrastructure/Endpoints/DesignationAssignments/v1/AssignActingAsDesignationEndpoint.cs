using FSH.Starter.WebApi.HumanResources.Application.EmployeeDesignationAssignments.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.DesignationAssignments.v1;

/// <summary>
/// Endpoint for assigning an "Acting As" (temporary) designation to an employee.
/// </summary>
public static class AssignActingAsDesignationEndpoint
{
    internal static RouteHandlerBuilder MapAssignActingAsDesignationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/acting-as", async (AssignActingAsDesignationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AssignActingAsDesignationEndpoint))
            .WithSummary("Assigns an acting as designation to an employee")
            .WithDescription("Assigns a temporary acting designation to an employee")
            .Produces<AssignDesignationResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Assign, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

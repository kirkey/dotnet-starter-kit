using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents.v1;

/// <summary>
/// Endpoint for creating an employee dependent.
/// </summary>
public static class CreateEmployeeDependentEndpoint
{
    internal static RouteHandlerBuilder MapCreateEmployeeDependentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateEmployeeDependentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetEmployeeDependentEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateEmployeeDependentEndpoint))
            .WithSummary("Creates a new employee dependent")
            .WithDescription("Creates a new employee dependent (family member, beneficiary)")
            .Produces<CreateEmployeeDependentResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.Employees.Manage")
            .MapToApiVersion(1);
    }
}


using FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees.v1;

public static class CreateEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapCreateEmployeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateEmployeeCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetEmployeeEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateEmployeeEndpoint))
            .WithSummary("Creates a new employee")
            .WithDescription("Creates a new employee record in the system")
            .Produces<CreateEmployeeResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.Employees.Create")
            .MapToApiVersion(1);
    }
}


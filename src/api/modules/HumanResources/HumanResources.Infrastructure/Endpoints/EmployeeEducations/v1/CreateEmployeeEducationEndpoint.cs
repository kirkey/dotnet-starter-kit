using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeEducations.v1;

/// <summary>
/// Endpoint for creating a new employee education record.
/// </summary>
public static class CreateEmployeeEducationEndpoint
{
    internal static RouteHandlerBuilder MapCreateEmployeeEducationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateEmployeeEducationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetEmployeeEducationEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateEmployeeEducationEndpoint))
            .WithSummary("Creates a new employee education record")
            .WithDescription("Creates a new employee education record with qualification details")
            .Produces<CreateEmployeeEducationResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.EmployeeEducations.Create")
            .MapToApiVersion(1);
    }
}


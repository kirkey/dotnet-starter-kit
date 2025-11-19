using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Create.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts.v1;

/// <summary>
/// Endpoint for creating an employee contact.
/// </summary>
public static class CreateEmployeeContactEndpoint
{
    internal static RouteHandlerBuilder MapCreateEmployeeContactEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateEmployeeContactCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetEmployeeContactEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateEmployeeContactEndpoint))
            .WithSummary("Creates a new employee contact")
            .WithDescription("Creates a new employee contact (emergency, reference, family)")
            .Produces<CreateEmployeeContactResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}


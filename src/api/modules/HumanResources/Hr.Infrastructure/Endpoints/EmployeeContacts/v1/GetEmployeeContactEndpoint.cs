using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts.v1;

/// <summary>
/// Endpoint for getting an employee contact by ID.
/// </summary>
public static class GetEmployeeContactEndpoint
{
    internal static RouteHandlerBuilder MapGetEmployeeContactEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeContactRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetEmployeeContactEndpoint))
            .WithSummary("Gets employee contact by ID")
            .WithDescription("Retrieves employee contact details")
            .Produces<EmployeeContactResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}


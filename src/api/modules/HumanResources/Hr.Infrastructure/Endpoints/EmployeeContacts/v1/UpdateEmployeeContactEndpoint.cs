using Shared.Authorization;
using FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Update.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeContacts.v1;

/// <summary>
/// Endpoint for updating an employee contact.
/// </summary>
public static class UpdateEmployeeContactEndpoint
{
    internal static RouteHandlerBuilder MapUpdateEmployeeContactEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateEmployeeContactCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateEmployeeContactEndpoint))
            .WithSummary("Updates an employee contact")
            .WithDescription("Updates employee contact information")
            .Produces<UpdateEmployeeContactResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}


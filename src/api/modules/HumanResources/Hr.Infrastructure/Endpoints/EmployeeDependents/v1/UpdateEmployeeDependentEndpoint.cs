using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents.v1;

/// <summary>
/// Endpoint for updating an employee dependent.
/// </summary>
public static class UpdateEmployeeDependentEndpoint
{
    internal static RouteHandlerBuilder MapUpdateEmployeeDependentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateEmployeeDependentCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateEmployeeDependentEndpoint))
            .WithSummary("Updates an employee dependent")
            .WithDescription("Updates employee dependent information")
            .Produces<UpdateEmployeeDependentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Manage, FshResources.Employees))
            .MapToApiVersion(1);
    }
}


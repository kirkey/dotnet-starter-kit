using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Delete.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeePayComponents;

public static class DeleteEmployeePayComponentEndpoint
{
    internal static RouteHandlerBuilder MapDeleteEmployeePayComponentEndpoint(this RouteGroupBuilder group)
    {
        return group.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new DeleteEmployeePayComponentCommand(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(DeleteEmployeePayComponentEndpoint))
        .WithSummary("Delete employee pay component")
        .WithDescription("Deletes employee pay component assignment by ID")
        .Produces<DeleteEmployeePayComponentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}


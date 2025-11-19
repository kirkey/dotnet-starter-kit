using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeePayComponents;

public static class UpdateEmployeePayComponentEndpoint
{
    internal static RouteHandlerBuilder MapUpdateEmployeePayComponentEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPut("/{id:guid}", async (DefaultIdType id, UpdateEmployeePayComponentCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var response = await mediator.Send(command).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateEmployeePayComponentEndpoint))
        .WithSummary("Update employee pay component")
        .WithDescription("Updates employee pay component assignment")
        .Produces<UpdateEmployeePayComponentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}


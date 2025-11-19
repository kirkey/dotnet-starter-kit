using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeePayComponents;

public static class CreateEmployeePayComponentEndpoint
{
    internal static RouteHandlerBuilder MapCreateEmployeePayComponentEndpoint(this RouteGroupBuilder group)
    {
        return group.MapPost("/", async (CreateEmployeePayComponentCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(CreateEmployeePayComponentEndpoint))
        .WithSummary("Create employee pay component")
        .WithDescription("Creates employee-specific pay component assignment")
        .Produces<CreateEmployeePayComponentResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Payroll))
        .MapToApiVersion(1);
    }
}


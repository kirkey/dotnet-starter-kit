using FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeePayComponents;

public static class GetEmployeePayComponentEndpoint
{
    internal static RouteHandlerBuilder MapGetEmployeePayComponentEndpoint(this RouteGroupBuilder group)
    {
        return group.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            var response = await mediator.Send(new GetEmployeePayComponentRequest(id)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetEmployeePayComponentEndpoint))
        .WithSummary("Get employee pay component by ID")
        .WithDescription("Retrieves employee pay component assignment by ID")
        .Produces<EmployeePayComponentResponse>()
        .RequirePermission("Permissions.EmployeePayComponents.View")
        .MapToApiVersion(1);
    }
}


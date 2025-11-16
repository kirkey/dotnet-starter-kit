using FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees.v1;

public static class GetEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapGetEmployeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetEmployeeEndpoint))
            .WithSummary("Gets employee by ID")
            .WithDescription("Retrieves employee details")
            .Produces<EmployeeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

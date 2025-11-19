using FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeDependents.v1;

/// <summary>
/// Endpoint for getting an employee dependent by ID.
/// </summary>
public static class GetEmployeeDependentEndpoint
{
    internal static RouteHandlerBuilder MapGetEmployeeDependentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeDependentRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetEmployeeDependentEndpoint))
            .WithSummary("Gets employee dependent by ID")
            .WithDescription("Retrieves employee dependent details")
            .Produces<EmployeeDependentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Employees))
            .MapToApiVersion(1);
    }
}


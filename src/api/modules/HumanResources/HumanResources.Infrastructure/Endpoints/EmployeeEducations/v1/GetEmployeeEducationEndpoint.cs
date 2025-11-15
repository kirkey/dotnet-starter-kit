using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.EmployeeEducations.v1;

/// <summary>
/// Endpoint for retrieving employee education details.
/// </summary>
public static class GetEmployeeEducationEndpoint
{
    internal static RouteHandlerBuilder MapGetEmployeeEducationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeEducationRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetEmployeeEducationEndpoint))
            .WithSummary("Gets employee education details")
            .WithDescription("Retrieves detailed information about a specific employee education record")
            .Produces<EmployeeEducationResponse>()
            .RequirePermission("Permissions.EmployeeEducations.View")
            .MapToApiVersion(1);
    }
}


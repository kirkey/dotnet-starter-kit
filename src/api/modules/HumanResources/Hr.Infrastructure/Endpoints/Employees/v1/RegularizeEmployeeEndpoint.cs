using FSH.Starter.WebApi.HumanResources.Application.Employees.Regularize.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Employees.v1;

/// <summary>
/// Endpoint for regularizing a probationary employee per Philippines Labor Code Article 280.
/// Converts Probationary status to Regular employment.
/// </summary>
public static class RegularizeEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapRegularizeEmployeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/regularize", async (DefaultIdType id, RegularizeEmployeeCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID mismatch");

                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(RegularizeEmployeeEndpoint))
            .WithSummary("Regularizes a probationary employee")
            .WithDescription("Regularizes a probationary employee per Philippines Labor Code Article 280. Typically after probation period (6-12 months).")
            .Produces<RegularizeEmployeeResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Regularize, FshResources.Employees))
            .MapToApiVersion(1);
    }
}

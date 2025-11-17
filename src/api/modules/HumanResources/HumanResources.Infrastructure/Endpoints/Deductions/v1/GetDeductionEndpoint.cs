using FSH.Starter.WebApi.HumanResources.Application.Deductions.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Deductions.v1;

/// <summary>
/// Endpoint for getting deduction details.
/// </summary>
public static class GetDeductionEndpoint
{
    internal static RouteHandlerBuilder MapGetDeductionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetDeductionRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetDeductionEndpoint))
            .WithSummary("Get Deduction Details")
            .WithDescription("Retrieves detailed information for the specified deduction type including recovery rules and compliance settings.")
            .Produces<DeductionResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Payroll))
            .MapToApiVersion(1);
    }
}


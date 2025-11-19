using FSH.Starter.WebApi.HumanResources.Application.Benefits.Get.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Benefits.v1;

/// <summary>
/// Endpoint for getting benefit details.
/// </summary>
public static class GetBenefitEndpoint
{
    internal static RouteHandlerBuilder MapGetBenefitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new GetBenefitRequest(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetBenefitEndpoint))
            .WithSummary("Get Benefit Details")
            .WithDescription("Retrieves detailed information for the specified benefit including contributions, coverage, and effective dates.")
            .Produces<BenefitResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


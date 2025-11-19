using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitEnrollments.v1;

/// <summary>
/// Endpoint for updating a benefit enrollment.
/// </summary>
public static class UpdateBenefitEnrollmentEndpoint
{
    internal static RouteHandlerBuilder MapUpdateBenefitEnrollmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdateBenefitEnrollmentCommand request, ISender mediator) =>
            {
                var updateRequest = request with { Id = id };
                var response = await mediator.Send(updateRequest).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBenefitEnrollmentEndpoint))
            .WithSummary("Updates a benefit enrollment")
            .WithDescription("Updates a benefit enrollment. Limited updates allowed depending on approval status.")
            .Produces<UpdateBenefitEnrollmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


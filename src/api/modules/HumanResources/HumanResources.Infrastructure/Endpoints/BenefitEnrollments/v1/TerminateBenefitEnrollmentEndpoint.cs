using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Terminate.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitEnrollments.v1;

/// <summary>
/// Endpoint for terminating a benefit enrollment.
/// </summary>
public static class TerminateBenefitEnrollmentEndpoint
{
    internal static RouteHandlerBuilder MapTerminateBenefitEnrollmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/terminate", async (DefaultIdType id, ISender mediator) =>
            {
                var request = new TerminateBenefitEnrollmentCommand(id);
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(TerminateBenefitEnrollmentEndpoint))
            .WithSummary("Terminates a benefit enrollment")
            .WithDescription("Terminates an active benefit enrollment, effective immediately or on a specified date.")
            .Produces<TerminateBenefitEnrollmentResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Terminate, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


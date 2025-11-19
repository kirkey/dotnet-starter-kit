using FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitEnrollments.v1;

/// <summary>
/// Endpoint for creating a new benefit enrollment.
/// </summary>
public static class CreateBenefitEnrollmentEndpoint
{
    internal static RouteHandlerBuilder MapCreateBenefitEnrollmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBenefitEnrollmentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetBenefitEnrollmentEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateBenefitEnrollmentEndpoint))
            .WithSummary("Creates a new benefit enrollment")
            .WithDescription("Creates a new benefit enrollment for an employee. Requires approval from HR.")
            .Produces<CreateBenefitEnrollmentResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


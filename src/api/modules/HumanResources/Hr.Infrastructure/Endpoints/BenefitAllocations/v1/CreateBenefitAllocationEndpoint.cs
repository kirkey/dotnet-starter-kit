using FSH.Starter.WebApi.HumanResources.Application.BenefitAllocations.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.BenefitAllocations.v1;

/// <summary>
/// Endpoint for creating a new benefit allocation.
/// </summary>
public static class CreateBenefitAllocationEndpoint
{
    internal static RouteHandlerBuilder MapCreateBenefitAllocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBenefitAllocationCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetBenefitAllocationEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateBenefitAllocationEndpoint))
            .WithSummary("Creates a new benefit allocation")
            .WithDescription("Creates a new benefit allocation for an employee. Allocates specific benefit amounts with optional HR approval.")
            .Produces<CreateBenefitAllocationResponse>(StatusCodes.Status201Created)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


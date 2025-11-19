using FSH.Starter.WebApi.HumanResources.Application.Benefits.Create.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Benefits.v1;

/// <summary>
/// Endpoint for creating a new benefit.
/// </summary>
public static class CreateBenefitEndpoint
{
    internal static RouteHandlerBuilder MapCreateBenefitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBenefitCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.CreatedAtRoute(nameof(GetBenefitEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateBenefitEndpoint))
            .WithSummary("Create Benefit")
            .WithDescription("Creates a new benefit offering (mandatory or optional) with contribution details per Philippines Labor Code.")
            .Produces<CreateBenefitResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


using FSH.Starter.WebApi.HumanResources.Application.Benefits.Update.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Benefits.v1;

/// <summary>
/// Endpoint for updating an existing benefit.
/// </summary>
public static class UpdateBenefitEndpoint
{
    internal static RouteHandlerBuilder MapUpdateBenefitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id}", async (DefaultIdType id, UpdateBenefitCommand body, ISender mediator) =>
            {
                if (body.Id != id)
                    return Results.BadRequest(new { title = "ID mismatch." });

                var response = await mediator.Send(body).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBenefitEndpoint))
            .WithSummary("Update Benefit")
            .WithDescription("Updates benefit contribution, coverage, activation status, and description.")
            .Produces<UpdateBenefitResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


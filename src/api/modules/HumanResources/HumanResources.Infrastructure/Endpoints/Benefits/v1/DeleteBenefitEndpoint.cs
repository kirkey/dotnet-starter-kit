using FSH.Starter.WebApi.HumanResources.Application.Benefits.Delete.v1;
using Shared.Authorization;

namespace FSH.Starter.WebApi.HumanResources.Infrastructure.Endpoints.Benefits.v1;

/// <summary>
/// Endpoint for deleting a benefit.
/// </summary>
public static class DeleteBenefitEndpoint
{
    internal static RouteHandlerBuilder MapDeleteBenefitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var command = new DeleteBenefitCommand(id);
                var response = await mediator.Send(command).ConfigureAwait(false);
                return response.Success ? Results.Ok(response) : Results.NotFound();
            })
            .WithName(nameof(DeleteBenefitEndpoint))
            .WithSummary("Delete Benefit")
            .WithDescription("Deletes a benefit from the catalog.")
            .Produces<DeleteBenefitResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Benefits))
            .MapToApiVersion(1);
    }
}


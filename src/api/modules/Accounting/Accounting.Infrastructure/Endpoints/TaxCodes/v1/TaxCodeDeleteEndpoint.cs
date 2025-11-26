using Accounting.Application.TaxCodes.Delete.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.TaxCodes.v1;

public static class TaxCodeDeleteEndpoint
{
    internal static RouteHandlerBuilder MapTaxCodeDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                await mediator.Send(new DeleteTaxCodeCommand(id)).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(TaxCodeDeleteEndpoint))
            .WithSummary("Delete a tax code")
            .WithDescription("Delete a tax code by ID")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

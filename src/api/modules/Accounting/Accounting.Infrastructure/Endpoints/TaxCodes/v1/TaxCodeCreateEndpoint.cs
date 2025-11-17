using Accounting.Application.TaxCodes.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.TaxCodes.v1;

public static class TaxCodeCreateEndpoint
{
    internal static RouteHandlerBuilder MapTaxCodeCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateTaxCodeCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(TaxCodeCreateEndpoint))
            .WithSummary("Create a tax code")
            .WithDescription("Create a new tax code with rate and jurisdiction")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

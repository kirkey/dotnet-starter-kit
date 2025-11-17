using Accounting.Application.CreditMemos.Get;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoGetEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var query = new GetCreditMemoQuery(id);
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreditMemoGetEndpoint))
            .WithSummary("Get credit memo by ID")
            .WithDescription("Retrieve a specific credit memo by its identifier")
            .Produces<Accounting.Application.CreditMemos.Responses.CreditMemoResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

using Accounting.Application.InterCompanyTransactions.Get;
using Accounting.Application.InterCompanyTransactions.Responses;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.InterCompanyTransactions.v1;

public static class InterCompanyTransactionGetEndpoint
{
    internal static RouteHandlerBuilder MapInterCompanyTransactionGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInterCompanyTransactionRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(InterCompanyTransactionGetEndpoint))
            .WithSummary("Get inter-company transaction by ID")
            .WithDescription("Retrieves an inter-company transaction by its unique identifier")
            .Produces<InterCompanyTransactionResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


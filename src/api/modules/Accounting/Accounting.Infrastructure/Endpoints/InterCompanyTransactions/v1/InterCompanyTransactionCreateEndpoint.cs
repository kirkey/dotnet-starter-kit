using Accounting.Application.InterCompanyTransactions.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.InterCompanyTransactions.v1;

public static class InterCompanyTransactionCreateEndpoint
{
    internal static RouteHandlerBuilder MapInterCompanyTransactionCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (InterCompanyTransactionCreateCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/intercompany-transactions/{response.Id}", response);
            })
            .WithName(nameof(InterCompanyTransactionCreateEndpoint))
            .WithSummary("Create inter-company transaction")
            .Produces<InterCompanyTransactionCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


using Accounting.Application.DebitMemos.Create;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.DebitMemos.v1;

public static class DebitMemoCreateEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateDebitMemoCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DebitMemoCreateEndpoint))
            .WithSummary("Create a debit memo")
            .WithDescription("Create a new debit memo for receivable/payable adjustments")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

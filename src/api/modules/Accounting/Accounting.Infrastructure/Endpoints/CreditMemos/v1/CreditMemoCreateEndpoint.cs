using Accounting.Application.CreditMemos.Create;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CreditMemos.v1;

public static class CreditMemoCreateEndpoint
{
    internal static RouteHandlerBuilder MapCreditMemoCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateCreditMemoCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CreditMemoCreateEndpoint))
            .WithSummary("Create a credit memo")
            .WithDescription("Create a new credit memo for receivable/payable adjustments")
            .WithTags("Credit Memos")
            .Produces<DefaultIdType>()
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}

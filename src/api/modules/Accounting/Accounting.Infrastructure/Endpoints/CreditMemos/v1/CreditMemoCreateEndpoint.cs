using Accounting.Application.CreditMemos.Create;

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
            .Produces<DefaultIdType>()
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

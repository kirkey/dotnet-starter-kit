using Accounting.Application.DebitMemos.Create;

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
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(1);
    }
}

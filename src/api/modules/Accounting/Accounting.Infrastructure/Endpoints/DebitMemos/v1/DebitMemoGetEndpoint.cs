using Accounting.Application.DebitMemos.Get;

namespace Accounting.Infrastructure.Endpoints.DebitMemos.v1;

public static class DebitMemoGetEndpoint
{
    internal static RouteHandlerBuilder MapDebitMemoGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var query = new GetDebitMemoQuery(id);
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DebitMemoGetEndpoint))
            .WithSummary("Get debit memo by ID")
            .WithDescription("Retrieve a specific debit memo by its identifier")
            .Produces<Accounting.Application.DebitMemos.Responses.DebitMemoResponse>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

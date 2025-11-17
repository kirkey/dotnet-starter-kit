using Accounting.Application.PrepaidExpenses.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;

public static class PrepaidExpenseUpdateEndpoint
{
    internal static RouteHandlerBuilder MapPrepaidExpenseUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, UpdatePrepaidExpenseCommand request, ISender mediator) =>
            {
                var command = request with { Id = id };
                var expenseId = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(new { Id = expenseId });
            })
            .WithName(nameof(PrepaidExpenseUpdateEndpoint))
            .WithSummary("Update prepaid expense")
            .WithDescription("Updates a prepaid expense details")
            .Produces<object>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


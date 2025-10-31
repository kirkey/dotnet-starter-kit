using Accounting.Application.PrepaidExpenses.Get;
using Accounting.Application.PrepaidExpenses.Responses;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;

public static class PrepaidExpenseGetEndpoint
{
    internal static RouteHandlerBuilder MapPrepaidExpenseGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPrepaidExpenseRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PrepaidExpenseGetEndpoint))
            .WithSummary("Get prepaid expense by ID")
            .WithDescription("Retrieves a prepaid expense by its unique identifier")
            .Produces<PrepaidExpenseResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}


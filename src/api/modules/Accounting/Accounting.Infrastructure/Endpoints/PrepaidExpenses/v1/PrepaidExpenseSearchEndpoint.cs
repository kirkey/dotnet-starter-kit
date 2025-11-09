using Accounting.Application.PrepaidExpenses.Responses;
using Accounting.Application.PrepaidExpenses.Search.v1;

namespace Accounting.Infrastructure.Endpoints.PrepaidExpenses.v1;

public static class PrepaidExpenseSearchEndpoint
{
    internal static RouteHandlerBuilder MapPrepaidExpenseSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPrepaidExpensesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PrepaidExpenseSearchEndpoint))
            .WithSummary("Search prepaid expenses")
            .WithDescription("Search prepaid expenses with filtering and pagination")
            .Produces<PagedList<PrepaidExpenseResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}



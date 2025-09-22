using Accounting.Application.Accruals.Search;
using Accounting.Application.Accruals.Responses;

// Endpoint for searching accruals
namespace Accounting.Infrastructure.Endpoints.Accruals.v1;

public static class AccrualSearchEndpoint
{
    internal static RouteHandlerBuilder MapAccrualSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchAccrualsQuery query, ISender mediator) =>
            {
                var response = await mediator.Send(query).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AccrualSearchEndpoint))
            .WithSummary("Search accruals")
            .WithDescription("Search accrual entries with filters and pagination")
            .Produces<List<AccrualResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}

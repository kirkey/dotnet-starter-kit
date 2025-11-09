using Accounting.Application.Payments.Search.v1;

namespace Accounting.Infrastructure.Endpoints.Payments.v1;

/// <summary>
/// Endpoint for searching payments.
/// </summary>
public static class PaymentSearchEndpoint
{
    /// <summary>
    /// Maps the payment search endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapPaymentSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (PaymentSearchRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PaymentSearchEndpoint))
            .WithSummary("Search payments")
            .WithDescription("Searches payments with filtering and pagination")
            .Produces<PagedList<PaymentSearchResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}



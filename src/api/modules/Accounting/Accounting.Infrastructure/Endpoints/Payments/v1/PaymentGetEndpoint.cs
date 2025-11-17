using Accounting.Application.Payments.Get.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payments.v1;

/// <summary>
/// Endpoint for retrieving a payment by ID.
/// </summary>
public static class PaymentGetEndpoint
{
    /// <summary>
    /// Maps the payment retrieval endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapPaymentGetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new PaymentGetQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(PaymentGetEndpoint))
            .WithSummary("Get payment by ID")
            .WithDescription("Retrieves a payment by its unique identifier")
            .Produces<PaymentGetResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}



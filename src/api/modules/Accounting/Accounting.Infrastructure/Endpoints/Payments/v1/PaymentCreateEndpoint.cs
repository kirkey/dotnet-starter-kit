using Accounting.Application.Payments.Create.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Payments.v1;

/// <summary>
/// Endpoint for creating a new payment.
/// </summary>
public static class PaymentCreateEndpoint
{
    /// <summary>
    /// Maps the payment creation endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapPaymentCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePaymentCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Created($"/accounting/payments/{response.Id}", response);
            })
            .WithName(nameof(PaymentCreateEndpoint))
            .WithSummary("Create a new payment")
            .WithDescription("Creates a new payment record for customer/member payments")
            .Produces<PaymentCreateResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}


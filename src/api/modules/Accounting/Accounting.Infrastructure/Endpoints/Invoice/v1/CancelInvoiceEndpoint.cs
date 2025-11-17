using Accounting.Application.Invoices.Cancel.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for cancelling an invoice.
/// </summary>
public static class CancelInvoiceEndpoint
{
    /// <summary>
    /// Maps the cancel invoice endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapCancelInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (DefaultIdType id, [FromBody] CancelInvoiceCommand command, ISender mediator) =>
            {
                if (id != command.InvoiceId)
                {
                    return Results.BadRequest("Invoice ID in URL does not match the request body.");
                }

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CancelInvoiceEndpoint))
            .WithSummary("Cancel an invoice")
            .WithDescription("Cancels an unpaid invoice with an optional reason.")
            .Produces<CancelInvoiceResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}


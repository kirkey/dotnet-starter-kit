using Accounting.Application.Invoices.Send.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for sending an invoice to customer.
/// </summary>
public static class SendInvoiceEndpoint
{
    /// <summary>
    /// Maps the send invoice endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapSendInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/send", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new SendInvoiceCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SendInvoiceEndpoint))
            .WithSummary("Send an invoice")
            .WithDescription("Transitions invoice status from Draft to Sent.")
            .Produces<SendInvoiceResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}


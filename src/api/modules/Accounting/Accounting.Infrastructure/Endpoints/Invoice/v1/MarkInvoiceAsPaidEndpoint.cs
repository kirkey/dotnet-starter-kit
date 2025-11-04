using Accounting.Application.Invoices.MarkPaid.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for marking an invoice as paid.
/// </summary>
public static class MarkInvoiceAsPaidEndpoint
{
    /// <summary>
    /// Maps the mark invoice as paid endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapMarkInvoiceAsPaidEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-paid", async (DefaultIdType id, [FromBody] MarkInvoiceAsPaidCommand command, ISender mediator) =>
            {
                if (id != command.InvoiceId)
                {
                    return Results.BadRequest("Invoice ID in URL does not match the request body.");
                }

                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkInvoiceAsPaidEndpoint))
            .WithSummary("Mark invoice as paid")
            .WithDescription("Marks an invoice as fully paid with payment details.")
            .Produces<MarkInvoiceAsPaidResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Update")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}


using Accounting.Application.Invoices.Delete.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for deleting an invoice.
/// </summary>
public static class DeleteInvoiceEndpoint
{
    /// <summary>
    /// Maps the delete invoice endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapDeleteInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteInvoiceCommand(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteInvoiceEndpoint))
            .WithSummary("Delete an invoice")
            .WithDescription("Deletes an invoice. Only Draft or Cancelled invoices can be deleted.")
            .Produces<DeleteInvoiceResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.Delete")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}


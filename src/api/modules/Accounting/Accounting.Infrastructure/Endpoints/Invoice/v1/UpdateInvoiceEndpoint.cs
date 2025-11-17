using Accounting.Application.Invoices.Update.v1;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for updating an existing invoice.
/// </summary>
public static class UpdateInvoiceEndpoint
{
    /// <summary>
    /// Maps the invoice update endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapUpdateInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (DefaultIdType id, [FromBody] UpdateInvoiceCommand request, ISender mediator) =>
            {
                var command = request with { InvoiceId = id };
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInvoiceEndpoint))
            .WithSummary("Update an invoice")
            .WithDescription("Updates an existing invoice. Only Draft invoices can be modified.")
            .Produces<UpdateInvoiceResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Accounting))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}


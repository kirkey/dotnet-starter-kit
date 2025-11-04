using Accounting.Application.Invoices.Create.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for creating new invoices in the accounting system.
/// </summary>
public static class InvoiceCreateEndpoint
{
    /// <summary>
    /// Maps the invoice creation endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapInvoiceCreateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInvoiceCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Created($"/accounting/invoices/{response.Id}", response);
            })
            .WithName(nameof(InvoiceCreateEndpoint))
            .WithSummary("Create a new invoice")
            .WithDescription("Creates a new invoice in the accounts receivable system with comprehensive validation.")
            .Produces<CreateInvoiceResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequirePermission("Permissions.Accounting.Create")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}


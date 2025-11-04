using Accounting.Application.Invoices.Get.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for getting a specific invoice by ID.
/// </summary>
public static class GetInvoiceEndpoint
{
    /// <summary>
    /// Maps the get invoice endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapGetInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInvoiceRequest(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetInvoiceEndpoint))
            .WithSummary("Get invoice by ID")
            .WithDescription("Retrieves a specific invoice by its unique identifier.")
            .Produces<InvoiceResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}


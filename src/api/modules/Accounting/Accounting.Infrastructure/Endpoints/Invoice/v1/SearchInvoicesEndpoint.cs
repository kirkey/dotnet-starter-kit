using Accounting.Application.Invoices.Get.v1;
using Accounting.Application.Invoices.Search.v1;

namespace Accounting.Infrastructure.Endpoints.Invoice.v1;

/// <summary>
/// Endpoint for searching invoices with filters and pagination.
/// </summary>
public static class SearchInvoicesEndpoint
{
    internal static RouteHandlerBuilder MapSearchInvoicesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async ([FromBody] SearchInvoicesRequest request, ISender mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInvoicesEndpoint))
            .WithSummary("Search invoices")
            .WithDescription("Search and filter invoices with pagination.")
            .Produces<PagedList<InvoiceResponse>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}


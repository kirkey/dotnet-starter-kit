using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders.v1;

/// <summary>
/// Endpoint for generating a PDF report for a purchase order.
/// </summary>
public static class GeneratePurchaseOrderPdfEndpoint
{
    /// <summary>
    /// Maps the generate purchase order PDF endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGeneratePurchaseOrderPdfEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}/pdf", async (DefaultIdType id, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new GeneratePurchaseOrderPdfCommand(id);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    $"PurchaseOrder_{id}_{DateTime.UtcNow:yyyyMMddHHmmss}.pdf");
            })
            .WithName(nameof(GeneratePurchaseOrderPdfEndpoint))
            .WithSummary("Generate PDF report for a purchase order")
            .WithDescription("Generates a professional PDF report for the specified purchase order including all items and approval information.")
            .Produces<FileResult>()
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);

        return app;
    }
}


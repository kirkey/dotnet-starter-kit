using FSH.Framework.Core.Exceptions;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1.Services;
using FSH.Starter.WebApi.Store.Application.PurchaseOrders.Specs;

namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1;

/// <summary>
/// Handler for generating a PDF report for a purchase order.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GeneratePurchaseOrderPdfHandler"/> class.
/// </remarks>
/// <param name="repository">Purchase order repository.</param>
/// <param name="pdfService">PDF generation service.</param>
public sealed class GeneratePurchaseOrderPdfHandler(
    IRepository<PurchaseOrder> repository,
    IPurchaseOrderPdfService pdfService) : IRequestHandler<GeneratePurchaseOrderPdfCommand, byte[]>
{
    /// <summary>
    /// Handles the command to generate a PDF report.
    /// </summary>
    /// <param name="request">The command request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>PDF file as byte array.</returns>
    public async Task<byte[]> Handle(GeneratePurchaseOrderPdfCommand request, CancellationToken cancellationToken)
    {
        var spec = new PurchaseOrderByIdWithItemsSpec(request.PurchaseOrderId);
        var purchaseOrder = await repository.FirstOrDefaultAsync(spec, cancellationToken)
            ?? throw new NotFoundException($"Purchase Order with ID {request.PurchaseOrderId} not found.");

        return pdfService.GeneratePdf(purchaseOrder);
    }
}


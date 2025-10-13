namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1.Services;

/// <summary>
/// Service for generating PDF reports for purchase orders.
/// </summary>
public interface IPurchaseOrderPdfService
{
    /// <summary>
    /// Generates a PDF document for the specified purchase order.
    /// </summary>
    /// <param name="purchaseOrder">The purchase order to generate PDF for.</param>
    /// <returns>PDF file as byte array.</returns>
    byte[] GeneratePdf(PurchaseOrder purchaseOrder);
}


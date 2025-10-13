namespace FSH.Starter.WebApi.Store.Application.PurchaseOrders.Report.v1;

/// <summary>
/// Command to generate a PDF report for a purchase order.
/// </summary>
public sealed record GeneratePurchaseOrderPdfCommand(DefaultIdType PurchaseOrderId) : IRequest<byte[]>;


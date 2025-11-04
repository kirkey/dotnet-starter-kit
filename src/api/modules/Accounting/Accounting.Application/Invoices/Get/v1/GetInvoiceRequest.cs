namespace Accounting.Application.Invoices.Get.v1;

/// <summary>
/// Request to get a specific invoice by ID.
/// </summary>
/// <param name="Id">Invoice identifier.</param>
public sealed record GetInvoiceRequest(DefaultIdType Id) : IRequest<InvoiceResponse>;


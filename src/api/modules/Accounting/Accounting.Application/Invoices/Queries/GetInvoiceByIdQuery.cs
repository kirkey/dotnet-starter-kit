using Accounting.Application.Invoices.Responses;

namespace Accounting.Application.Invoices.Queries;

/// <summary>
/// Query to get a specific invoice by ID.
/// </summary>
/// <param name="Id">Invoice identifier.</param>
public sealed record GetInvoiceByIdQuery(DefaultIdType Id) : IRequest<InvoiceResponse>;

using Accounting.Domain.Entities;

namespace Accounting.Application.Invoices.Cancel;

/// <summary>
/// Handler for cancelling an invoice.
/// </summary>
public sealed class CancelInvoiceHandler(
    ILogger<CancelInvoiceHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<CancelInvoiceCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(CancelInvoiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken);
        
        if (invoice == null)
        {
            throw new NotFoundException($"Invoice with id {request.InvoiceId} not found");
        }

        invoice.Cancel(request.CancellationReason);

        await repository.UpdateAsync(invoice, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Invoice {InvoiceId} cancelled. Reason: {Reason}", 
            request.InvoiceId, request.CancellationReason ?? "Not specified");

        return invoice.Id;
    }
}

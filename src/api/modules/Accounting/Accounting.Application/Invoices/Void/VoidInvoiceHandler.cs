using Accounting.Domain.Entities;

namespace Accounting.Application.Invoices.Void;

/// <summary>
/// Handler for voiding an invoice.
/// </summary>
public sealed class VoidInvoiceHandler(
    ILogger<VoidInvoiceHandler> logger,
    [FromKeyedServices("accounting:invoices")] IRepository<Invoice> repository)
    : IRequestHandler<VoidInvoiceCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(VoidInvoiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var invoice = await repository.GetByIdAsync(request.InvoiceId, cancellationToken);
        
        if (invoice == null)
        {
            throw new NotFoundException($"Invoice with id {request.InvoiceId} not found");
        }

        invoice.Void(request.VoidReason);

        await repository.UpdateAsync(invoice, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Invoice {InvoiceId} voided. Reason: {Reason}", 
            request.InvoiceId, request.VoidReason ?? "Not specified");

        return invoice.Id;
    }
}

namespace Accounting.Application.Bills.Update.v1;

/// <summary>
/// Handler for updating a bill.
/// </summary>
public sealed class BillUpdateHandler(
    ILogger<BillUpdateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<Bill> repository)
    : IRequestHandler<BillUpdateCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(BillUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var bill = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new BillByIdNotFoundException(request.Id);

        bill.Update(
            dueDate: request.DueDate,
            subtotalAmount: request.SubtotalAmount,
            taxAmount: request.TaxAmount,
            shippingAmount: request.ShippingAmount,
            paymentTerms: request.PaymentTerms,
            description: request.Description,
            notes: request.Notes);

        await repository.UpdateAsync(bill, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Bill updated {BillId} - {BillNumber}", bill.Id, bill.BillNumber);
        return bill.Id;
    }
}


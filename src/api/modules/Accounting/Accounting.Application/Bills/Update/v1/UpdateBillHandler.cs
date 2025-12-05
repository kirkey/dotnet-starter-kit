namespace Accounting.Application.Bills.Update.v1;

/// <summary>
/// Handler for updating an existing bill.
/// </summary>
public sealed class UpdateBillHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository,
    ILogger<UpdateBillHandler> logger)
    : IRequestHandler<UpdateBillCommand, UpdateBillResponse>
{
    public async Task<UpdateBillResponse> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Updating bill {BillId}", request.BillId);

        var bill = await repository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        bill.Update(
            request.BillNumber,
            request.BillDate,
            request.DueDate,
            request.Description,
            request.PeriodId,
            request.PaymentTerms,
            request.PurchaseOrderNumber,
            request.Notes);

        await repository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bill {BillId} updated successfully", request.BillId);

        return new UpdateBillResponse(bill.Id);
    }
}


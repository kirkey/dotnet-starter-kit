namespace Accounting.Application.Bills.Delete.v1;

/// <summary>
/// Handler for deleting a bill.
/// Only draft bills can be deleted.
/// </summary>
public sealed class DeleteBillHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository,
    ILogger<DeleteBillHandler> logger)
    : IRequestHandler<DeleteBillCommand, DeleteBillResponse>
{
    public async Task<DeleteBillResponse> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Deleting bill {BillId}", request.BillId);

        var bill = await repository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        // Cannot delete posted or paid bills
        if (bill.IsPosted)
            throw new BillCannotBeModifiedException(request.BillId, "Cannot delete posted bill");

        if (bill.IsPaid)
            throw new BillCannotBeModifiedException(request.BillId, "Cannot delete paid bill");

        await repository.DeleteAsync(bill, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bill {BillId} deleted successfully", request.BillId);

        return new DeleteBillResponse(request.BillId);
    }
}

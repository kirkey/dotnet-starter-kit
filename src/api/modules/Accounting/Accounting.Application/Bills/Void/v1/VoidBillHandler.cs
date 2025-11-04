namespace Accounting.Application.Bills.Void.v1;

/// <summary>
/// Handler for voiding a bill.
/// </summary>
public sealed class VoidBillHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository,
    ILogger<VoidBillHandler> logger)
    : IRequestHandler<VoidBillCommand, VoidBillResponse>
{
    public async Task<VoidBillResponse> Handle(VoidBillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Voiding bill {BillId}", request.BillId);

        var bill = await repository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        bill.Void(request.Reason);

        await repository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bill {BillId} voided successfully", request.BillId);

        return new VoidBillResponse(request.BillId);
    }
}


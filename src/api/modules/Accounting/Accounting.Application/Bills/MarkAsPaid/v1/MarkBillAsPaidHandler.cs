namespace Accounting.Application.Bills.MarkAsPaid.v1;

/// <summary>
/// Handler for marking a bill as paid.
/// </summary>
public sealed class MarkBillAsPaidHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository,
    ILogger<MarkBillAsPaidHandler> logger)
    : IRequestHandler<MarkBillAsPaidCommand, MarkBillAsPaidResponse>
{
    public async Task<MarkBillAsPaidResponse> Handle(MarkBillAsPaidCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Marking bill {BillId} as paid on {PaidDate}", request.BillId, request.PaidDate);

        var bill = await repository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        bill.MarkAsPaid(request.PaidDate);

        await repository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bill {BillId} marked as paid successfully", request.BillId);

        return new MarkBillAsPaidResponse(request.BillId);
    }
}


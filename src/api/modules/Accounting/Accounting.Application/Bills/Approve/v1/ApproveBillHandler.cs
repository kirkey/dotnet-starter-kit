namespace Accounting.Application.Bills.Approve.v1;

/// <summary>
/// Handler for approving a bill.
/// </summary>
public sealed class ApproveBillHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository,
    ILogger<ApproveBillHandler> logger)
    : IRequestHandler<ApproveBillCommand, ApproveBillResponse>
{
    public async Task<ApproveBillResponse> Handle(ApproveBillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Approving bill {BillId} by {ApprovedBy}", request.BillId, request.ApprovedBy);

        var bill = await repository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        bill.Approve(request.ApprovedBy);

        await repository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bill {BillId} approved successfully by {ApprovedBy}", request.BillId, request.ApprovedBy);

        return new ApproveBillResponse(request.BillId);
    }
}


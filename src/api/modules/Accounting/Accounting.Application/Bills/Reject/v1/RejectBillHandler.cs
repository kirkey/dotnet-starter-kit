using Accounting.Domain.Entities;
using Accounting.Domain.Exceptions;

namespace Accounting.Application.Bills.Reject.v1;

/// <summary>
/// Handler for rejecting a bill.
/// </summary>
public sealed class RejectBillHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository,
    ILogger<RejectBillHandler> logger)
    : IRequestHandler<RejectBillCommand, RejectBillResponse>
{
    public async Task<RejectBillResponse> Handle(RejectBillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Rejecting bill {BillId} by {RejectedBy}", request.BillId, request.RejectedBy);

        var bill = await repository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        bill.Reject(request.RejectedBy, request.Reason);

        await repository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bill {BillId} rejected by {RejectedBy}", request.BillId, request.RejectedBy);

        return new RejectBillResponse(request.BillId);
    }
}


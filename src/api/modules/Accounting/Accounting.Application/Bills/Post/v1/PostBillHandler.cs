using Accounting.Domain.Entities;
using Accounting.Domain.Exceptions;

namespace Accounting.Application.Bills.Post.v1;

/// <summary>
/// Handler for posting a bill to the general ledger.
/// </summary>
public sealed class PostBillHandler(
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository,
    ILogger<PostBillHandler> logger)
    : IRequestHandler<PostBillCommand, PostBillResponse>
{
    public async Task<PostBillResponse> Handle(PostBillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Posting bill {BillId} to general ledger", request.BillId);

        var bill = await repository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        bill.Post();

        await repository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bill {BillId} posted successfully to general ledger", request.BillId);

        return new PostBillResponse(request.BillId);
    }
}


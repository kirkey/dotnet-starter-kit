using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.Bills.Approve.v1;

/// <summary>
/// Handler for approving a bill.
/// </summary>
public sealed class ApproveBillHandler(
    ICurrentUser currentUser,
    [FromKeyedServices("accounting:bills")] IRepository<Bill> repository,
    ILogger<ApproveBillHandler> logger)
    : IRequestHandler<ApproveBillCommand, ApproveBillResponse>
{
    public async Task<ApproveBillResponse> Handle(ApproveBillCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();
        
        logger.LogInformation("Approving bill {BillId} by user {ApproverId}", request.BillId, approverId);

        var bill = await repository.GetByIdAsync(request.BillId, cancellationToken).ConfigureAwait(false)
            ?? throw new BillNotFoundException(request.BillId);

        bill.Approve(approverId, approverName);

        await repository.UpdateAsync(bill, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Bill {BillId} approved successfully by user {ApproverId}", request.BillId, approverId);

        return new ApproveBillResponse(request.BillId);
    }
}


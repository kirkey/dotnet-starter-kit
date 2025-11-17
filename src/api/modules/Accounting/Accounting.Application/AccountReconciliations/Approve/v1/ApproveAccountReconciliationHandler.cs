namespace Accounting.Application.AccountReconciliations.Approve.v1;

/// <summary>
/// Handler for approving account reconciliation.
/// </summary>
public sealed class ApproveAccountReconciliationHandler(
    [FromKeyedServices("accounting:account-reconciliations")] IRepository<AccountReconciliation> repository,
    ILogger<ApproveAccountReconciliationHandler> logger)
    : IRequestHandler<ApproveAccountReconciliationCommand>
{
    public async Task Handle(ApproveAccountReconciliationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var reconciliation = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (reconciliation == null)
            throw new AccountReconciliationNotFoundException(request.Id);

        if (reconciliation.Variance > 0)
            throw new CannotApproveReconciliationWithVarianceException(request.Id, reconciliation.Variance);

        reconciliation.Approve(request.ApproverId, request.ApproverName, request.Remarks);

        await repository.UpdateAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account reconciliation {Id} approved by {ApproverId}",
            request.Id, request.ApproverId);
    }
}


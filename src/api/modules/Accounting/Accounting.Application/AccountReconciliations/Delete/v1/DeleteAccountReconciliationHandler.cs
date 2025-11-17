namespace Accounting.Application.AccountReconciliations.Delete.v1;

/// <summary>
/// Handler for deleting account reconciliation.
/// </summary>
public sealed class DeleteAccountReconciliationHandler(
    [FromKeyedServices("accounting:account-reconciliations")] IRepository<AccountReconciliation> repository,
    ILogger<DeleteAccountReconciliationHandler> logger)
    : IRequestHandler<DeleteAccountReconciliationCommand>
{
    public async Task Handle(DeleteAccountReconciliationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var reconciliation = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (reconciliation == null)
            throw new AccountReconciliationNotFoundException(request.Id);

        if (reconciliation.ReconciliationStatus == "Approved")
            throw new InvalidOperationException("Cannot delete approved reconciliation.");

        await repository.DeleteAsync(reconciliation, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Account reconciliation {Id} deleted", request.Id);
    }
}


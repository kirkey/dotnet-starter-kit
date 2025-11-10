namespace Accounting.Application.GeneralLedgers.Update.v1;

/// <summary>
/// Handler for updating general ledger entry details.
/// </summary>
public sealed class GeneralLedgerUpdateHandler(
    [FromKeyedServices("accounting:general-ledger")] IRepository<GeneralLedger> repository,
    ILogger<GeneralLedgerUpdateHandler> logger)
    : IRequestHandler<GeneralLedgerUpdateCommand, DefaultIdType>
{
    /// <summary>
    /// Handles the general ledger update command.
    /// </summary>
    public async Task<DefaultIdType> Handle(GeneralLedgerUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Updating general ledger entry with ID {LedgerId}", request.Id);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null)
        {
            logger.LogWarning("General ledger entry with ID {LedgerId} not found", request.Id);
            throw new NotFoundException($"General ledger entry with ID {request.Id} not found");
        }

        // Use the entity's Update method
        entry.Update(
            debit: request.Debit,
            credit: request.Credit,
            memo: request.Memo,
            usoaClass: request.UsoaClass,
            referenceNumber: request.ReferenceNumber,
            description: request.Description,
            notes: request.Notes
        );

        await repository.UpdateAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("General ledger entry {LedgerId} updated successfully", entry.Id);

        return entry.Id;
    }
}

using Microsoft.Extensions.Logging;

namespace Accounting.Application.GeneralLedgers.Update.v1;

/// <summary>
/// Handler for updating general ledger entry details.
/// </summary>
public sealed class GeneralLedgerUpdateHandler : IRequestHandler<GeneralLedgerUpdateCommand, DefaultIdType>
{
    private readonly IRepository<GeneralLedger> _repository;
    private readonly ILogger<GeneralLedgerUpdateHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GeneralLedgerUpdateHandler"/> class.
    /// </summary>
    public GeneralLedgerUpdateHandler(
        IRepository<GeneralLedger> repository,
        ILogger<GeneralLedgerUpdateHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the general ledger update command.
    /// </summary>
    public async Task<DefaultIdType> Handle(GeneralLedgerUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Updating general ledger entry with ID {LedgerId}", request.Id);

        var entry = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null)
        {
            _logger.LogWarning("General ledger entry with ID {LedgerId} not found", request.Id);
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

        await _repository.UpdateAsync(entry, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("General ledger entry {LedgerId} updated successfully", entry.Id);

        return entry.Id;
    }
}

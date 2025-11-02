namespace Accounting.Application.GeneralLedgers.Commands.PostJournalEntry.v1;

using Domain.Exceptions;

public sealed class PostJournalEntryCommandHandler(
    ILogger<PostJournalEntryCommandHandler> logger,
    [FromKeyedServices("accounting:journalentries")]
    IRepository<JournalEntry> journalRepository,
    [FromKeyedServices("accounting:journal-lines")]
    IReadRepository<JournalEntryLine> journalLineRepository,
    [FromKeyedServices("accounting:generalledger")]
    IRepository<GeneralLedger> ledgerRepository,
    [FromKeyedServices("accounting:accounts")]
    IRepository<ChartOfAccount> accountRepository)
    : IRequestHandler<PostJournalEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(PostJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get the journal entry
        var journalEntry = await journalRepository.GetByIdAsync(request.JournalEntryId, cancellationToken);
        if (journalEntry == null)
        {
            throw new JournalEntryNotFoundException(request.JournalEntryId);
        }

        // Validate that journal entry is not already posted
        if (journalEntry.IsPosted)
        {
            throw new JournalEntryAlreadyPostedException(journalEntry.Id);
        }

        // Get all lines for this journal entry
        var spec = new JournalEntries.Lines.Specs.JournalEntryLinesByJournalEntryIdSpec(journalEntry.Id);
        var lines = await journalLineRepository.ListAsync(spec, cancellationToken);

        // Validate balances if required
        if (request.ValidateBalances)
        {
            var totalDebits = lines.Sum(l => l.DebitAmount);
            var totalCredits = lines.Sum(l => l.CreditAmount);
            if (totalDebits != totalCredits)
            {
                throw new JournalEntryUnbalancedException($"Journal entry {journalEntry.ReferenceNumber} is unbalanced. Debits: {totalDebits}, Credits: {totalCredits}");
            }
        }

        // Create general ledger entries
        foreach (var line in lines)
        {
            var account = await accountRepository.GetByIdAsync(line.AccountId, cancellationToken);
            var usoaClass = account?.UsoaCategory ?? "General";
            var ledgerEntry = GeneralLedger.Create(
                journalEntry.Id,
                line.AccountId,
                line.DebitAmount,
                line.CreditAmount,
                usoaClass,
                request.PostingDate,
                line.Memo,
                request.PostingReference ?? journalEntry.ReferenceNumber
            );
            await ledgerRepository.AddAsync(ledgerEntry, cancellationToken);
        }

        // Mark journal entry as posted

        await journalRepository.UpdateAsync(journalEntry, cancellationToken);

        logger.LogInformation("Journal entry {EntryNumber} posted successfully", journalEntry.ReferenceNumber);
        return journalEntry.Id;
    }
}

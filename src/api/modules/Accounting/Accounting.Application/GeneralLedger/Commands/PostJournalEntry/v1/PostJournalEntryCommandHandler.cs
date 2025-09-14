using Accounting.Application.JournalEntries.Exceptions;
using JournalEntryAlreadyPostedException = Accounting.Application.JournalEntries.Exceptions.JournalEntryAlreadyPostedException;
using JournalEntryNotFoundException = Accounting.Application.JournalEntries.Exceptions.JournalEntryNotFoundException;

namespace Accounting.Application.GeneralLedger.Commands.PostJournalEntry.v1;

public sealed class PostJournalEntryCommandHandler(
    ILogger<PostJournalEntryCommandHandler> logger,
    [FromKeyedServices("accounting:journalentries")]
    IRepository<JournalEntry> journalRepository,
    [FromKeyedServices("accounting:generalledger")]
    IRepository<Accounting.Domain.GeneralLedger> ledgerRepository,
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
            throw new JournalEntryNotFoundException($"Journal entry with ID {request.JournalEntryId} not found");
        }

        // Validate that journal entry is not already posted
        if (journalEntry.IsPosted)
        {
            throw new JournalEntryAlreadyPostedException($"Journal entry {journalEntry.ReferenceNumber} is already posted");
        }

        // Validate balances if required
        if (request.ValidateBalances)
        {
            var totalDebits = journalEntry.Lines.Sum(l => l.DebitAmount);
            var totalCredits = journalEntry.Lines.Sum(l => l.CreditAmount);
            if (totalDebits != totalCredits)
            {
                throw new JournalEntryUnbalancedException($"Journal entry {journalEntry.ReferenceNumber} is unbalanced. Debits: {totalDebits}, Credits: {totalCredits}");
            }
        }

        // Create general ledger entries
        foreach (var line in journalEntry.Lines)
        {
            var account = await accountRepository.GetByIdAsync(line.AccountId, cancellationToken);
            var usoaClass = account?.UsoaCategory ?? "General";
            var ledgerEntry = Accounting.Domain.GeneralLedger.Create(
                journalEntry.Id,
                line.AccountId,
                line.DebitAmount,
                line.CreditAmount,
                usoaClass,
                request.PostingDate,
                line.Description,
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

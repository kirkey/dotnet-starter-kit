using Accounting.Application.GeneralLedger.Commands.PostJournalEntry.v1;
using Accounting.Application.JournalEntries.Exceptions;
using Accounting.Domain;
using FSH.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Accounting.Application.GeneralLedger.Commands.PostJournalEntry.v1;

public sealed class PostJournalEntryCommandHandler(
    ILogger<PostJournalEntryCommandHandler> logger,
    [FromKeyedServices("accounting:journalentries")] IRepository<JournalEntry> journalRepository,
    [FromKeyedServices("accounting:generalledger")] IRepository<Domain.GeneralLedger> ledgerRepository)
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
            var totalDebits = journalEntry.Lines?.Where(l => l.DebitAmount > 0).Sum(l => l.DebitAmount) ?? 0;
            var totalCredits = journalEntry.Lines?.Where(l => l.CreditAmount > 0).Sum(l => l.CreditAmount) ?? 0;
            
            if (totalDebits != totalCredits)
            {
                throw new JournalEntryUnbalancedException($"Journal entry {journalEntry.ReferenceNumber} is unbalanced. Debits: {totalDebits}, Credits: {totalCredits}");
            }
        }

        // Create general ledger entries
        foreach (var line in journalEntry.Lines ?? [])
        {
            var ledgerEntry = GeneralLedger.Create(
                journalEntry.Id,
                line.AccountId,
                request.PostingDate,
                line.DebitAmount,
                line.CreditAmount,
                line.Description,
                request.PostingReference ?? journalEntry.ReferenceNumber);

            await ledgerRepository.AddAsync(ledgerEntry, cancellationToken);
        }

        // Mark journal entry as posted
        journalEntry.MarkAsPosted(request.PostingDate, request.PostingReference);
        await journalRepository.UpdateAsync(journalEntry, cancellationToken);

        logger.LogInformation("Journal entry {EntryNumber} posted successfully", journalEntry.ReferenceNumber);

        return journalEntry.Id;
    }
}

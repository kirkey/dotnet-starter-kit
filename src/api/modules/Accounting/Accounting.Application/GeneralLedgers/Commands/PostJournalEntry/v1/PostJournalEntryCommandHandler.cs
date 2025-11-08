using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.GeneralLedgers.Commands.PostJournalEntry.v1;

using Domain.Exceptions;

public sealed class PostJournalEntryCommandHandler(
    ILogger<PostJournalEntryCommandHandler> logger,
    ICurrentUser currentUser,
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

        // Get posted by user
        var postedBy = currentUser.GetUserEmail() ?? currentUser.Name ?? "System";

        // Create general ledger entries
        foreach (var line in lines)
        {
            var account = await accountRepository.GetByIdAsync(line.AccountId, cancellationToken);
            var accountCode = account?.AccountCode ?? line.AccountId.ToString();
            var usoaClass = account?.UsoaCategory ?? "General";
            
            var ledgerEntry = GeneralLedger.Create(
                journalEntry.Id,        // entryId
                line.AccountId,         // accountId
                accountCode,            // accountCode
                line.DebitAmount,       // debit
                line.CreditAmount,      // credit
                request.PostingDate,    // transactionDate
                usoaClass,              // usoaClass
                line.Memo,              // memo
                request.PostingReference ?? journalEntry.ReferenceNumber  // referenceNumber
            );
            
            // Post the ledger entry
            ledgerEntry.Post(postedBy);
            await ledgerRepository.AddAsync(ledgerEntry, cancellationToken);
        }

        // Mark journal entry as posted
        await journalRepository.UpdateAsync(journalEntry, cancellationToken);

        logger.LogInformation("Journal entry {EntryNumber} posted successfully by {User}", journalEntry.ReferenceNumber, postedBy);
        return journalEntry.Id;
    }
}

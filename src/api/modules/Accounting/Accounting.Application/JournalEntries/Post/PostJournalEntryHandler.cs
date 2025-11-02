using Accounting.Application.JournalEntries.Lines.Specs;

namespace Accounting.Application.JournalEntries.Post;


/// <summary>
/// Handler for posting a journal entry to the general ledger.
/// Validates the entry is balanced and updates the posted status.
/// </summary>
public sealed class PostJournalEntryHandler(
    ILogger<PostJournalEntryHandler> logger,
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository,
    [FromKeyedServices("accounting:journal-lines")] IReadRepository<JournalEntryLine> lineRepository)
    : IRequestHandler<PostJournalEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(PostJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var journalEntry = await repository.GetByIdAsync(request.JournalEntryId, cancellationToken);
        
        if (journalEntry == null)
        {
            throw new JournalEntryNotFoundException(request.JournalEntryId);
        }

        // Get all lines for this journal entry and validate balance
        var spec = new JournalEntryLinesByJournalEntryIdSpec(request.JournalEntryId);
        var lines = await lineRepository.ListAsync(spec, cancellationToken);

        var totalDebits = lines.Sum(l => l.DebitAmount);
        var totalCredits = lines.Sum(l => l.CreditAmount);

        if (Math.Abs(totalDebits - totalCredits) >= 0.01m)
        {
            throw new JournalEntryNotBalancedException(request.JournalEntryId);
        }

        // Post the journal entry
        journalEntry.Post();

        await repository.UpdateAsync(journalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Journal entry {JournalEntryId} posted to general ledger on {PostDate}", 
            request.JournalEntryId, journalEntry.Date);

        return journalEntry.Id;
    }
}


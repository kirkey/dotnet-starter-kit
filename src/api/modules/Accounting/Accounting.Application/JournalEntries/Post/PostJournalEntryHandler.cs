namespace Accounting.Application.JournalEntries.Post;

/// <summary>
/// Handler for posting a journal entry to the general ledger.
/// Validates the entry is balanced and updates the posted status.
/// </summary>
public sealed class PostJournalEntryHandler(
    ILogger<PostJournalEntryHandler> logger,
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
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

        // Post will validate balance internally and throw exception if not balanced
        journalEntry.Post();

        await repository.UpdateAsync(journalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Journal entry {JournalEntryId} posted to general ledger on {PostDate}", 
            request.JournalEntryId, journalEntry.Date);

        return journalEntry.Id;
    }
}


namespace Accounting.Application.JournalEntries.Reject;

/// <summary>
/// Handler for rejecting a journal entry.
/// </summary>
public sealed class RejectJournalEntryHandler(
    ILogger<RejectJournalEntryHandler> logger,
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<RejectJournalEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(RejectJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var journalEntry = await repository.GetByIdAsync(request.JournalEntryId, cancellationToken);
        
        if (journalEntry == null)
        {
            throw new JournalEntryNotFoundException(request.JournalEntryId);
        }

        if (string.IsNullOrWhiteSpace(request.RejectedBy))
        {
            throw new ArgumentException("RejectedBy is required for journal entry rejection.");
        }

        journalEntry.Reject(request.RejectedBy);

        await repository.UpdateAsync(journalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Journal entry {JournalEntryId} rejected by {RejectedBy}. Reason: {Reason}", 
            request.JournalEntryId, request.RejectedBy, request.RejectionReason ?? "Not specified");

        return journalEntry.Id;
    }
}

using Accounting.Application.JournalEntries.Exceptions;

namespace Accounting.Application.JournalEntries.Approve;

/// <summary>
/// Handler for approving a journal entry.
/// </summary>
public sealed class ApproveJournalEntryHandler(
    ILogger<ApproveJournalEntryHandler> logger,
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<ApproveJournalEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApproveJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var journalEntry = await repository.GetByIdAsync(request.JournalEntryId, cancellationToken);
        
        if (journalEntry == null)
        {
            throw new JournalEntryNotFoundException(request.JournalEntryId);
        }

        if (string.IsNullOrWhiteSpace(request.ApprovedBy))
        {
            throw new ArgumentException("ApprovedBy is required for journal entry approval.");
        }

        journalEntry.Approve(request.ApprovedBy);

        await repository.UpdateAsync(journalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Journal entry {JournalEntryId} approved by {ApprovedBy}", 
            request.JournalEntryId, request.ApprovedBy);

        return journalEntry.Id;
    }
}

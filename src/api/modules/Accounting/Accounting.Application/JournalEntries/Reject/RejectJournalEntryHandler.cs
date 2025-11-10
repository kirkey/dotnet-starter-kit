using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.JournalEntries.Reject;

/// <summary>
/// Handler for rejecting a journal entry.
/// </summary>
public sealed class RejectJournalEntryHandler(
    ILogger<RejectJournalEntryHandler> logger,
    ICurrentUser currentUser,
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

        var rejectorName = currentUser.GetUserName() ?? "Unknown";

        journalEntry.Reject(rejectorName);

        await repository.UpdateAsync(journalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Journal entry {JournalEntryId} rejected by {RejectedBy}. Reason: {Reason}", 
            request.JournalEntryId, rejectorName, request.RejectionReason ?? "Not specified");

        return journalEntry.Id;
    }
}

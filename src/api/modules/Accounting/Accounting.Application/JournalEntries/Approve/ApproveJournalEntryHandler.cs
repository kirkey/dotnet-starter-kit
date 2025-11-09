using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.JournalEntries.Approve;

/// <summary>
/// Handler for approving a journal entry.
/// </summary>
public sealed class ApproveJournalEntryHandler(
    ILogger<ApproveJournalEntryHandler> logger,
    ICurrentUser currentUser,
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<ApproveJournalEntryCommand, DefaultIdType>
{
    public async Task<DefaultIdType> Handle(ApproveJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Use spec to load entry with lines for balance validation
        var spec = new Specs.GetJournalEntryWithLinesSpec(request.JournalEntryId);
        var journalEntry = await ((IReadRepository<JournalEntry>)repository).FirstOrDefaultAsync(spec, cancellationToken);
        
        if (journalEntry == null)
        {
            throw new JournalEntryNotFoundException(request.JournalEntryId);
        }

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName();

        // Validate that the entry is balanced before approving
        journalEntry.ValidateBalance();

        journalEntry.Approve(approverId, approverName);

        await repository.UpdateAsync(journalEntry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Journal entry {JournalEntryId} approved by {ApprovedBy}", 
            request.JournalEntryId, approverName);

        return journalEntry.Id;
    }
}

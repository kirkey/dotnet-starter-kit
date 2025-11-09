using FSH.Framework.Core.Identity.Users.Abstractions;

namespace Accounting.Application.RecurringJournalEntries.Approve.v1;

public sealed class ApproveRecurringJournalEntryHandler(
    ILogger<ApproveRecurringJournalEntryHandler> logger,
    ICurrentUser currentUser,
    [FromKeyedServices("accounting")] IRepository<RecurringJournalEntry> repository)
    : IRequestHandler<ApproveRecurringJournalEntryCommand>
{
    public async Task Handle(ApproveRecurringJournalEntryCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var entry = await repository.GetByIdAsync(command.Id, cancellationToken).ConfigureAwait(false);
        if (entry == null)
            throw new RecurringJournalEntryNotFoundException(command.Id);

        var approverId = currentUser.GetUserId();
        var approverName = currentUser.GetUserName(); // or GetUserName() if available
        entry.Approve(approverId, approverName);

        await repository.UpdateAsync(entry, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Recurring journal entry {EntryId} approved by user {ApproverId}", command.Id, approverId);
    }
}

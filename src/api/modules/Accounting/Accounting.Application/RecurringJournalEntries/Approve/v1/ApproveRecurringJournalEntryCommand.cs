namespace Accounting.Application.RecurringJournalEntries.Approve.v1;

/// <summary>
/// Command to approve a recurring journal entry template.
/// The approver is automatically determined from the current user session.
/// </summary>
public sealed class ApproveRecurringJournalEntryCommand : IRequest
{
    public DefaultIdType Id { get; set; }
}

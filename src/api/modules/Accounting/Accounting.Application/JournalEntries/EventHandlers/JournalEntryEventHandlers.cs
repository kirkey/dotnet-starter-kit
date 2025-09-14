using Accounting.Domain.Events.JournalEntry;

namespace Accounting.Application.JournalEntries.EventHandlers;

public sealed class JournalEntryCreatedEventHandler(ILogger<JournalEntryCreatedEventHandler> logger)
    : INotificationHandler<JournalEntryCreated>
{
    public Task Handle(JournalEntryCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Journal Entry created: {Id} - {ReferenceNumber} - {Source}", 
            notification.Id, notification.ReferenceNumber, notification.Source);
        return Task.CompletedTask;
    }
}

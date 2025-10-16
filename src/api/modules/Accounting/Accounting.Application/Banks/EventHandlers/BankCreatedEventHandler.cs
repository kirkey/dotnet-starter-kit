using Accounting.Domain.Events.Bank;

namespace Accounting.Application.Banks.EventHandlers;

/// <summary>
/// Event handler for the BankCreated domain event.
/// Handles any side effects or notifications when a bank is created.
/// </summary>
public sealed class BankCreatedEventHandler : INotificationHandler<BankCreated>
{
    private readonly ILogger<BankCreatedEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BankCreatedEventHandler"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public BankCreatedEventHandler(ILogger<BankCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the BankCreated event.
    /// </summary>
    /// <param name="notification">The bank created event.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task Handle(BankCreated notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Bank created event handled: BankId={BankId}, BankCode={BankCode}, Name={Name}",
            notification.Id,
            notification.BankCode,
            notification.Name);

        // Additional side effects can be implemented here, such as:
        // - Sending notifications
        // - Creating default bank accounts
        // - Updating external systems
        // - Publishing integration events

        return Task.CompletedTask;
    }
}


namespace Accounting.Domain.Events.Payee;

public class PayeeUpdatedEventHandler(
    ILogger<PayeeUpdatedEventHandler> logger,
    ICacheService cache)
    : DomainEventHandler<PayeeUpdated>(logger, cache, "payee")
{ }

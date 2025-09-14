namespace Accounting.Domain.Events.ChartOfAccount;

public class ChartOfAccountCreatedEventHandler(
    ILogger<ChartOfAccountCreatedEventHandler> logger,
    ICacheService cache)
    : DomainEventHandler<ChartOfAccountCreated>(logger, cache, "chartOfAccount");

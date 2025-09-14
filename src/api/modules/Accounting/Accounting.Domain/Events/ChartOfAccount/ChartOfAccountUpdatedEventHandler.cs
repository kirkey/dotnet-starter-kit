namespace Accounting.Domain.Events.ChartOfAccount;

public class ChartOfAccountUpdatedEventHandler(
    ILogger<ChartOfAccountUpdatedEventHandler> logger,
    ICacheService cache)
    : DomainEventHandler<ChartOfAccountUpdated>(logger, cache, "chartOfAccount");

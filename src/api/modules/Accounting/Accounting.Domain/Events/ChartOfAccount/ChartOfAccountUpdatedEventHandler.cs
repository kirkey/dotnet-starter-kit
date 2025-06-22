using FSH.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace Accounting.Domain.Events.ChartOfAccount;

public class ChartOfAccountUpdatedEventHandler(
    ILogger<ChartOfAccountUpdatedEventHandler> logger,
    ICacheService cache)
    : DomainEventHandler<ChartOfAccountUpdated>(logger, cache, "chartOfAccount");

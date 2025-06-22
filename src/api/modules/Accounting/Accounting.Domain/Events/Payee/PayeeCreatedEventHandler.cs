using FSH.Framework.Core.Caching;
using Microsoft.Extensions.Logging;

namespace Accounting.Domain.Events.Payee;

public class PayeeCreatedEventHandler(
    ILogger<PayeeCreatedEventHandler> logger,
    ICacheService cache)
    : DomainEventHandler<PayeeCreated>(logger, cache, "payee");

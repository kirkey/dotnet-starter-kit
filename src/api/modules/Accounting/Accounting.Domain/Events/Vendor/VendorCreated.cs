using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Vendor;

public record VendorCreated(DefaultIdType Id) : DomainEvent;


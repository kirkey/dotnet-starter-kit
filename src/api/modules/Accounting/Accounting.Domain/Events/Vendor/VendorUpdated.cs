using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Vendor;

public record VendorUpdated(Accounting.Domain.Vendor Vendor) : DomainEvent;

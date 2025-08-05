using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Vendor;

public record VendorActivated(DefaultIdType Id, string VendorCode, string Name) : DomainEvent;
public record VendorDeactivated(DefaultIdType Id, string VendorCode, string Name) : DomainEvent;

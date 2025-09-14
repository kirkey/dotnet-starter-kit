namespace Accounting.Domain.Events.Vendor;

public record VendorCreated(DefaultIdType Id, string VendorCode, string Name, string? Email, string? Terms, string? Description, string? Notes) : DomainEvent;

public record VendorUpdated(DefaultIdType Id, Accounting.Domain.Vendor Vendor) : DomainEvent;

public record VendorDeleted(DefaultIdType Id) : DomainEvent;

public record VendorStatusChanged(DefaultIdType Id, string VendorCode, string Status) : DomainEvent;


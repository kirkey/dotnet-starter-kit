using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Vendor;

public record VendorCreated(DefaultIdType Id, string VendorCode, string Name, string? Email, string? Terms, string? Description, string? Notes) : DomainEvent;

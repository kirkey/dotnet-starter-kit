namespace Accounting.Application.Vendors.Search.v1;

public record VendorSearchResponse(
    DefaultIdType Id,
    string VendorCode,
    string Name,
    string? ContactPerson,
    string? Email,
    string? Address,
    string? ExpenseAccountCode,
    string? ExpenseAccountName,
    string? Tin,
    string? Phone,
    string? Description,
    string? Notes
);

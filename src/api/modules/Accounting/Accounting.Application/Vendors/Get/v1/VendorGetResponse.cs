namespace Accounting.Application.Vendors.Get.v1;

public record VendorGetResponse(
    DefaultIdType Id,
    string VendorCode,
    string Name,
    string? Address,
    string? ExpenseAccountCode,
    string? ExpenseAccountName,
    string? Tin,
    string? Phone,
    string? Description,
    string? Notes
);

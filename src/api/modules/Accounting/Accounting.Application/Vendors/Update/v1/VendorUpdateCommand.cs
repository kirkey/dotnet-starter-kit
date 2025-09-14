namespace Accounting.Application.Vendors.Update.v1;

public record VendorUpdateCommand(
    DefaultIdType Id,
    string? VendorCode,
    string? Name,
    string? Address,
    string? BillingAddress,
    string? ContactPerson,
    string? Email,
    string? Terms,
    string? ExpenseAccountCode,
    string? ExpenseAccountName,
    string? Tin,
    string? Phone,
    string? Description,
    string? Notes
) : IRequest<VendorUpdateResponse>;

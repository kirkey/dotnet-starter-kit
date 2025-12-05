namespace Accounting.Application.Vendors.Create.v1;

public record CreateVendorCommand(
    string VendorCode,
    string Name,
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
) : IRequest<VendorCreateResponse>;

using MediatR;

namespace Accounting.Application.Vendors.Update.v1;

public record VendorUpdateCommand(
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
) : IRequest<VendorUpdateResponse>;

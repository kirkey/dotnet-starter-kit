using MediatR;

namespace Accounting.Application.Vendors.Create.v1;

public record VendorCreateCommand(
    string VendorCode,
    string Name,
    string? Address,
    string? ExpenseAccountCode,
    string? ExpenseAccountName,
    string? Tin,
    string? Phone,
    string? Description,
    string? Notes
) : IRequest<VendorCreateResponse>;

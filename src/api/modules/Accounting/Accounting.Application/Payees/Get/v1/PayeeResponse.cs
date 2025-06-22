namespace Accounting.Application.Payees.Get.v1;
public sealed record PayeeResponse(
    DefaultIdType Id,
    string PayeeCode,
    string Name,
    string? Address,
    string? ExpenseAccountCode,
    string? ExpenseAccountName,
    string? Tin,
    string? Description,
    string? Notes);

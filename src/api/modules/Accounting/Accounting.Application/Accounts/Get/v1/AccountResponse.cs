namespace Accounting.Application.Accounts.Get.v1;

public sealed record AccountResponse(
    DefaultIdType Id, string Name,
    string AccountCategory, string Type, string ParentCode, string Code, decimal Balance,
    string? remarks, string? Status, string? Description, string? Notes, string? FilePath);

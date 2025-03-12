using Accounting.Domain.Enums;
using FSH.Framework.Core.Dto;

namespace Accounting.Application.Accounts.Get.v1;

public sealed record AccountResponse(
    DefaultIdType Id, string Name,
    Category Category, TransactionType TransactionType, string ParentCode, string Code, decimal Balance,
    string? remarks, string? Status, string? Description, string? Notes, string? FilePath);

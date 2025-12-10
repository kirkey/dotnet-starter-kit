namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Get.v1;

public sealed record BranchResponse(
    DefaultIdType Id,
    string Code,
    string Name,
    string BranchType,
    string Status,
    DefaultIdType? ParentBranchId,
    string? Address,
    string? Phone,
    string? Email,
    string? ManagerName,
    string? ManagerPhone,
    string? ManagerEmail,
    DateOnly? OpeningDate,
    DateOnly? ClosingDate,
    decimal? CashHoldingLimit,
    string? Notes);

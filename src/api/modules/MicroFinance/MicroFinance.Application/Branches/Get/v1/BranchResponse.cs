namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Get.v1;

public sealed record BranchResponse(
    Guid Id,
    string Code,
    string Name,
    string BranchType,
    string Status,
    Guid? ParentBranchId,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? PostalCode,
    string? Phone,
    string? Email,
    string? ManagerName,
    string? ManagerPhone,
    string? ManagerEmail,
    DateOnly? OpeningDate,
    DateOnly? ClosingDate,
    decimal? CashHoldingLimit,
    string? Notes);

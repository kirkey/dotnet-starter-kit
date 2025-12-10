namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Search.v1;

public sealed record BranchSummaryResponse(
    DefaultIdType Id,
    string Code,
    string Name,
    string BranchType,
    string Status,
    string? ManagerName,
    DateOnly? OpeningDate);

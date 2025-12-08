namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Create.v1;

/// <summary>
/// Response after creating a branch.
/// </summary>
public sealed record CreateBranchResponse(DefaultIdType Id, string Code, string Name);

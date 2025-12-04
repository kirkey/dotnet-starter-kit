namespace FSH.Starter.WebApi.MicroFinance.Application.BranchTargets.Create.v1;

/// <summary>
/// Response from creating a branch target.
/// </summary>
public sealed record CreateBranchTargetResponse(Guid Id, string TargetType, decimal TargetValue, string Status);

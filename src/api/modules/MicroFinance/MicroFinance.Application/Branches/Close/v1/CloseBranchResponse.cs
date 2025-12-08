namespace FSH.Starter.WebApi.MicroFinance.Application.Branches.Close.v1;

public sealed record CloseBranchResponse(DefaultIdType Id, string Status, DateOnly ClosingDate);

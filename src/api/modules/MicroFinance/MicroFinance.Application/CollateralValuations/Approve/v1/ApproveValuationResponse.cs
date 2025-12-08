namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralValuations.Approve.v1;

public sealed record ApproveValuationResponse(DefaultIdType Id, string Status, DateOnly? ApprovedDate);

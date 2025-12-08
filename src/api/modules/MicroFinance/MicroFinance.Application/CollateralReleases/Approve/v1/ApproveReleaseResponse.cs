namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Approve.v1;

public sealed record ApproveReleaseResponse(DefaultIdType Id, string Status, DateOnly ApprovedDate);

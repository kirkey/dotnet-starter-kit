namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralReleases.Approve.v1;

public sealed record ApproveReleaseResponse(Guid Id, string Status, DateOnly ApprovedDate);

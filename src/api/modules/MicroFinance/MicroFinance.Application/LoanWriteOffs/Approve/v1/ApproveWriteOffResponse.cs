namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Approve.v1;

public sealed record ApproveWriteOffResponse(DefaultIdType Id, string Status, DateOnly WriteOffDate);

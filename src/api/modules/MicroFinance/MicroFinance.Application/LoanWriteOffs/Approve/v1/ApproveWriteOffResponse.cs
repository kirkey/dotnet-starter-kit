namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Approve.v1;

public sealed record ApproveWriteOffResponse(Guid Id, string Status, DateOnly WriteOffDate);

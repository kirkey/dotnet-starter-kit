namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Transfer.v1;

public sealed record TransferAssignmentResponse(DefaultIdType OldAssignmentId, DefaultIdType NewAssignmentId, string Status);

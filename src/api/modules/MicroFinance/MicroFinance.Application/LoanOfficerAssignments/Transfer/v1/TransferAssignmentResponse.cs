namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Transfer.v1;

public sealed record TransferAssignmentResponse(Guid OldAssignmentId, Guid NewAssignmentId, string Status);

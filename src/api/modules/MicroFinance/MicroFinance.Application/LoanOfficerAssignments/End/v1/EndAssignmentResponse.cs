namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.End.v1;

public sealed record EndAssignmentResponse(Guid Id, string Status, DateOnly EndDate);

using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.End.v1;

public sealed record EndAssignmentCommand(
    Guid Id,
    DateOnly? EndDate = null,
    string? Reason = null) : IRequest<EndAssignmentResponse>;

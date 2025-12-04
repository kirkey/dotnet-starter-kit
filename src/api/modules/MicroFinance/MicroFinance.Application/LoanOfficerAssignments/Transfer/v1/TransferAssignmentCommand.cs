using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Transfer.v1;

public sealed record TransferAssignmentCommand(
    Guid Id,
    Guid NewStaffId,
    string? Reason = null) : IRequest<TransferAssignmentResponse>;

using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerAssignments.Transfer.v1;

public sealed record TransferAssignmentCommand(
    DefaultIdType Id,
    DefaultIdType NewStaffId,
    string? Reason = null) : IRequest<TransferAssignmentResponse>;

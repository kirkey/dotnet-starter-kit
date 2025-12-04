using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Approve.v1;

public sealed record ApproveWriteOffCommand(
    Guid Id,
    Guid UserId,
    string ApproverName,
    DateOnly WriteOffDate) : IRequest<ApproveWriteOffResponse>;

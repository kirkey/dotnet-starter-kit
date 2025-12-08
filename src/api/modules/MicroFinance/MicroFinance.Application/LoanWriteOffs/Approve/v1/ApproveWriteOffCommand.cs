using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Approve.v1;

public sealed record ApproveWriteOffCommand(
    DefaultIdType Id,
    DefaultIdType UserId,
    string ApproverName,
    DateOnly WriteOffDate) : IRequest<ApproveWriteOffResponse>;

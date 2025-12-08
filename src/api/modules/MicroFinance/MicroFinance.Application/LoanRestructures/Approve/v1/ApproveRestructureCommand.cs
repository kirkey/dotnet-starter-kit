using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Approve.v1;

public sealed record ApproveRestructureCommand(
    DefaultIdType Id,
    DefaultIdType UserId,
    string ApproverName,
    DateOnly EffectiveDate) : IRequest<ApproveRestructureResponse>;

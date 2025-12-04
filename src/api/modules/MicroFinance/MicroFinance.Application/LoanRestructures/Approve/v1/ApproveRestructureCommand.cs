using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Approve.v1;

public sealed record ApproveRestructureCommand(
    Guid Id,
    Guid UserId,
    string ApproverName,
    DateOnly EffectiveDate) : IRequest<ApproveRestructureResponse>;

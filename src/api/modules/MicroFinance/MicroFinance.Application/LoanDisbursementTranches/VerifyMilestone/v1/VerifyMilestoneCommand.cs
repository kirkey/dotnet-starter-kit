using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.VerifyMilestone.v1;

public sealed record VerifyMilestoneCommand(Guid Id) : IRequest<VerifyMilestoneResponse>;

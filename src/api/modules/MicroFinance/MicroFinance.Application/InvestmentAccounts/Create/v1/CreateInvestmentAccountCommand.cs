using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Create.v1;

public sealed record CreateInvestmentAccountCommand(
    DefaultIdType MemberId,
    string AccountNumber,
    string RiskProfile,
    DefaultIdType? AssignedAdvisorId = null,
    string? InvestmentGoal = null) : IRequest<CreateInvestmentAccountResponse>;

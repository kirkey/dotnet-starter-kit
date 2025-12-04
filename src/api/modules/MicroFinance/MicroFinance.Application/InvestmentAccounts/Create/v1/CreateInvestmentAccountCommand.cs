using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Create.v1;

public sealed record CreateInvestmentAccountCommand(
    Guid MemberId,
    string AccountNumber,
    string RiskProfile,
    Guid? AssignedAdvisorId = null,
    string? InvestmentGoal = null) : IRequest<CreateInvestmentAccountResponse>;

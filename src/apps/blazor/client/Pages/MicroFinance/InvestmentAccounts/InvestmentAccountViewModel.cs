using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentAccounts;

public class InvestmentAccountViewModel
{
    public DefaultIdType Id { get; set; }
    public DefaultIdType MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string RiskProfile { get; set; } = string.Empty;
    public DefaultIdType? AssignedAdvisorId { get; set; }
    public string? AssignedAdvisorName { get; set; }
    public string? InvestmentGoal { get; set; }
    public decimal TotalInvested { get; set; }
    public decimal CurrentValue { get; set; }
    public decimal UnrealizedGainLoss { get; set; }
    public decimal RealizedGainLoss { get; set; }
    public string? Status { get; set; }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<InvestmentAccountResponse, InvestmentAccountViewModel>();
            config.NewConfig<InvestmentAccountViewModel, CreateInvestmentAccountCommand>();
        }
    }
}

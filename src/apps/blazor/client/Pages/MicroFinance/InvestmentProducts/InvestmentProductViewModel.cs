using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;

namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.InvestmentProducts;

public class InvestmentProductViewModel
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
    public decimal MinimumInvestment { get; set; }
    public decimal ManagementFeePercent { get; set; }
    public decimal ExpectedReturnMin { get; set; }
    public decimal ExpectedReturnMax { get; set; }
    public int LockInPeriodDays { get; set; }
    public string? Description { get; set; }
    public decimal? CurrentNav { get; set; }
    public DateOnly? NavDate { get; set; }
    public bool IsActive { get; set; }

    public class Mapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<InvestmentProductResponse, InvestmentProductViewModel>();
            config.NewConfig<InvestmentProductViewModel, CreateInvestmentProductCommand>();
        }
    }
}

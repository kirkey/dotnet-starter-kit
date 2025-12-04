using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Create.v1;

public sealed record CreateInvestmentProductCommand(
    string Name,
    string Code,
    string ProductType,
    string RiskLevel,
    decimal MinimumInvestment,
    decimal ManagementFeePercent,
    decimal ExpectedReturnMin,
    decimal ExpectedReturnMax,
    int LockInPeriodDays = 0,
    string? Description = null) : IRequest<CreateInvestmentProductResponse>;

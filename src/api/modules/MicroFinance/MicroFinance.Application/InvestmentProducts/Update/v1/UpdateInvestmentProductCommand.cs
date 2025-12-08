using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentProducts.Update.v1;

public sealed record UpdateInvestmentProductCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    decimal? MinimumInvestment,
    decimal? MaximumInvestment,
    decimal? ManagementFeePercent,
    decimal? PerformanceFeePercent,
    decimal? EntryLoadPercent,
    decimal? ExitLoadPercent,
    int? MinimumHoldingDays,
    string? FundManager,
    string? Benchmark,
    bool? AllowPartialRedemption,
    bool? AllowSip,
    int? DisplayOrder) : IRequest<UpdateInvestmentProductResponse>;

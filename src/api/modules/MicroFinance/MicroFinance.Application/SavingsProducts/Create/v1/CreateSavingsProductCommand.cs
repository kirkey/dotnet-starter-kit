using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Create.v1;

public sealed record CreateSavingsProductCommand(
    [property: DefaultValue("REGULAR")] string Code,
    [property: DefaultValue("Regular Savings")] string Name,
    [property: DefaultValue("Standard savings account with competitive interest")] string? Description,
    [property: DefaultValue("USD")] string CurrencyCode,
    [property: DefaultValue(3.5)] decimal InterestRate,
    [property: DefaultValue("Monthly")] string InterestCalculation,
    [property: DefaultValue("Monthly")] string InterestPostingFrequency,
    [property: DefaultValue(0)] decimal MinOpeningBalance,
    [property: DefaultValue(100)] decimal MinBalanceForInterest,
    [property: DefaultValue(0)] decimal MinWithdrawalAmount,
    [property: DefaultValue(null)] decimal? MaxWithdrawalPerDay,
    [property: DefaultValue(false)] bool AllowOverdraft,
    [property: DefaultValue(null)] decimal? OverdraftLimit) : IRequest<CreateSavingsProductResponse>;

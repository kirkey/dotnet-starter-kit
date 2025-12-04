using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Update.v1;

/// <summary>
/// Command to update a savings product.
/// </summary>
public sealed record UpdateSavingsProductCommand(
    Guid Id,
    string? Name,
    string? Description,
    decimal? InterestRate,
    string? InterestCalculation,
    string? InterestPostingFrequency,
    decimal? MinOpeningBalance,
    decimal? MinBalanceForInterest,
    decimal? MinWithdrawalAmount,
    decimal? MaxWithdrawalPerDay,
    bool? AllowOverdraft,
    decimal? OverdraftLimit) : IRequest<UpdateSavingsProductResponse>;

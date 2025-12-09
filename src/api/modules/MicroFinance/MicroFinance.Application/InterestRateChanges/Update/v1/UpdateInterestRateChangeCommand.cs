using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Update.v1;

/// <summary>
/// Command to update a pending interest rate change.
/// </summary>
public sealed record UpdateInterestRateChangeCommand(
    DefaultIdType Id,
    string? ChangeType = null,
    DateOnly? EffectiveDate = null,
    decimal? NewRate = null,
    string? ChangeReason = null,
    string? Notes = null) : IRequest<UpdateInterestRateChangeResponse>;

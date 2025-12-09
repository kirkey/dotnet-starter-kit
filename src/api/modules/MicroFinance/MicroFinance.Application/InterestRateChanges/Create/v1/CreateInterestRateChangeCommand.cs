using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Create.v1;

/// <summary>
/// Command to create a new interest rate change request.
/// </summary>
public sealed record CreateInterestRateChangeCommand(
    DefaultIdType LoanId,
    string Reference,
    string ChangeType,
    DateOnly EffectiveDate,
    decimal PreviousRate,
    decimal NewRate,
    string ChangeReason,
    DateOnly? RequestDate = null,
    string? Notes = null) : IRequest<CreateInterestRateChangeResponse>;

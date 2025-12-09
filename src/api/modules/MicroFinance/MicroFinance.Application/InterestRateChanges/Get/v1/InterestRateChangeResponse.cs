namespace FSH.Starter.WebApi.MicroFinance.Application.InterestRateChanges.Get.v1;

/// <summary>
/// Response containing interest rate change details.
/// </summary>
public sealed record InterestRateChangeResponse(
    DefaultIdType Id,
    DefaultIdType LoanId,
    string Reference,
    string ChangeType,
    DateOnly RequestDate,
    DateOnly EffectiveDate,
    decimal PreviousRate,
    decimal NewRate,
    decimal RateChange,
    string ChangeReason,
    string Status,
    DefaultIdType? ApprovedByUserId,
    string? ApprovedBy,
    DateOnly? ApprovalDate,
    DateOnly? AppliedDate,
    string? RejectionReason,
    string? Notes);

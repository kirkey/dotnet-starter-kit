namespace FSH.Starter.WebApi.MicroFinance.Application.FeeWaivers.Get.v1;

/// <summary>
/// Response containing fee waiver details.
/// </summary>
public sealed record FeeWaiverResponse(
    DefaultIdType Id,
    DefaultIdType FeeChargeId,
    string Reference,
    string WaiverType,
    DateOnly RequestDate,
    decimal OriginalAmount,
    decimal WaivedAmount,
    decimal RemainingAmount,
    string WaiverReason,
    string Status,
    DefaultIdType? ApprovedByUserId,
    string? ApprovedBy,
    DateOnly? ApprovalDate,
    string? RejectionReason,
    string? Notes);

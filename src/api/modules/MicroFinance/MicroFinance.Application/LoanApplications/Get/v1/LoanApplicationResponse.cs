namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Get.v1;

/// <summary>
/// Response containing loan application details.
/// </summary>
public sealed record LoanApplicationResponse(
    DefaultIdType Id,
    string ApplicationNumber,
    DefaultIdType MemberId,
    DefaultIdType LoanProductId,
    DefaultIdType? MemberGroupId,
    decimal RequestedAmount,
    decimal? ApprovedAmount,
    int RequestedTermMonths,
    int? ApprovedTermMonths,
    string? Purpose,
    string Status,
    DateOnly ApplicationDate,
    DefaultIdType? AssignedOfficerId,
    DateTime? AssignedAt,
    DateTime? DecisionAt,
    DefaultIdType? DecisionByUserId,
    string? RejectionReason,
    DefaultIdType? LoanId);

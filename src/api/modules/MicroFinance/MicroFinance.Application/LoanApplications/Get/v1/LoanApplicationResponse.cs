namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Get.v1;

/// <summary>
/// Response containing loan application details.
/// </summary>
public sealed record LoanApplicationResponse(
    Guid Id,
    string ApplicationNumber,
    Guid MemberId,
    Guid LoanProductId,
    Guid? MemberGroupId,
    decimal RequestedAmount,
    decimal? ApprovedAmount,
    int RequestedTermMonths,
    int? ApprovedTermMonths,
    string? Purpose,
    string Status,
    DateOnly ApplicationDate,
    Guid? AssignedOfficerId,
    DateTime? AssignedAt,
    DateTime? DecisionAt,
    Guid? DecisionByUserId,
    string? RejectionReason,
    Guid? LoanId);

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;

public sealed record LoanDisbursementTrancheResponse(
    DefaultIdType Id,
    DefaultIdType LoanId,
    int TrancheSequence,
    string TrancheNumber,
    DateOnly ScheduledDate,
    DateOnly? DisbursedDate,
    decimal Amount,
    decimal Deductions,
    decimal NetAmount,
    string DisbursementMethod,
    string? BankAccountNumber,
    string? BankName,
    string? ReferenceNumber,
    string? Milestone,
    bool MilestoneVerified,
    string Status,
    DefaultIdType? ApprovedByUserId,
    DateTime? ApprovedAt,
    DefaultIdType? DisbursedByUserId);

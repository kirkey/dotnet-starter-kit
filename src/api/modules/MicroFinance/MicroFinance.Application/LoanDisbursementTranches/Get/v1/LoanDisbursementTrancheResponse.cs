namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;

public sealed record LoanDisbursementTrancheResponse(
    Guid Id,
    Guid LoanId,
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
    Guid? ApprovedByUserId,
    DateTime? ApprovedAt,
    Guid? DisbursedByUserId);

namespace FSH.Starter.WebApi.MicroFinance.Application.FixedDeposits.Get.v1;

public sealed record FixedDepositResponse(
    Guid Id,
    string CertificateNumber,
    Guid MemberId,
    Guid? SavingsProductId,
    Guid? LinkedSavingsAccountId,
    decimal PrincipalAmount,
    decimal InterestRate,
    int TermMonths,
    DateOnly DepositDate,
    DateOnly MaturityDate,
    decimal InterestEarned,
    decimal InterestPaid,
    string MaturityInstruction,
    string Status,
    DateOnly? ClosedDate,
    string? Notes
);

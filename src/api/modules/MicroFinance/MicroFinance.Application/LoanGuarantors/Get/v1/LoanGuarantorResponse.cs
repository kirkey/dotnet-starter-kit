namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;

public sealed record LoanGuarantorResponse(
    DefaultIdType Id,
    DefaultIdType LoanId,
    DefaultIdType GuarantorMemberId,
    decimal GuaranteedAmount,
    string? Relationship,
    DateOnly GuaranteeDate,
    DateOnly? ExpiryDate,
    string Status,
    string? Notes
);

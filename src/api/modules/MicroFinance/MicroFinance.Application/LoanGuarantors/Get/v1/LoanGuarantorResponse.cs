namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;

public sealed record LoanGuarantorResponse(
    Guid Id,
    Guid LoanId,
    Guid GuarantorMemberId,
    decimal GuaranteedAmount,
    string? Relationship,
    DateOnly GuaranteeDate,
    DateOnly? ExpiryDate,
    string Status,
    string? Notes
);

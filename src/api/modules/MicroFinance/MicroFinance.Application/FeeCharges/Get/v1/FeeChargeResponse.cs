namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Get.v1;

public sealed record FeeChargeResponse(
    DefaultIdType Id,
    DefaultIdType FeeDefinitionId,
    DefaultIdType MemberId,
    DefaultIdType? LoanId,
    DefaultIdType? SavingsAccountId,
    DefaultIdType? ShareAccountId,
    string Reference,
    DateOnly ChargeDate,
    DateOnly? DueDate,
    decimal Amount,
    decimal AmountPaid,
    decimal Outstanding,
    string Status,
    DateOnly? PaidDate,
    string? Notes
);

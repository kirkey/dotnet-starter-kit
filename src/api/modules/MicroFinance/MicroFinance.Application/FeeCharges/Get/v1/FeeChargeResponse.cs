namespace FSH.Starter.WebApi.MicroFinance.Application.FeeCharges.Get.v1;

public sealed record FeeChargeResponse(
    Guid Id,
    Guid FeeDefinitionId,
    Guid MemberId,
    Guid? LoanId,
    Guid? SavingsAccountId,
    Guid? ShareAccountId,
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

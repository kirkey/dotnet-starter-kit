namespace FSH.Starter.WebApi.MicroFinance.Application.PromiseToPays.Get.v1;

/// <summary>
/// Response containing promise to pay details.
/// </summary>
public sealed record PromiseToPayResponse(
    Guid Id,
    Guid CollectionCaseId,
    Guid LoanId,
    Guid MemberId,
    DateOnly PromiseDate,
    DateOnly PromisedPaymentDate,
    decimal PromisedAmount,
    decimal ActualAmountPaid,
    DateOnly? ActualPaymentDate,
    string Status,
    int RescheduleCount,
    string? BreachReason);

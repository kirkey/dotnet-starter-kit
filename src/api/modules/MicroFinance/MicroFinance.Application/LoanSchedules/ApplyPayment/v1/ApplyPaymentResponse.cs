namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.ApplyPayment.v1;

public sealed record ApplyPaymentResponse(
    DefaultIdType ScheduleId,
    decimal PaidAmount,
    bool IsPaid,
    DateOnly? PaidDate);

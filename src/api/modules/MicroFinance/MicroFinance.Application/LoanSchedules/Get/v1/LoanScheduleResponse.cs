namespace FSH.Starter.WebApi.MicroFinance.Application.LoanSchedules.Get.v1;

public sealed record LoanScheduleResponse(
    Guid Id,
    Guid LoanId,
    int InstallmentNumber,
    DateOnly DueDate,
    decimal PrincipalAmount,
    decimal InterestAmount,
    decimal TotalAmount,
    decimal PaidAmount,
    bool IsPaid,
    DateOnly? PaidDate
);

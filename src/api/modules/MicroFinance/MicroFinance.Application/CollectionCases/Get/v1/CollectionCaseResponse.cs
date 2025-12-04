namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Get.v1;

public sealed record CollectionCaseResponse(
    Guid Id,
    string CaseNumber,
    Guid LoanId,
    Guid MemberId,
    Guid? AssignedCollectorId,
    string Status,
    string Priority,
    string Classification,
    DateOnly OpenedDate,
    DateOnly? AssignedDate,
    DateOnly? ClosedDate,
    int DaysPastDueAtOpen,
    int CurrentDaysPastDue,
    decimal AmountOverdue,
    decimal TotalOutstanding,
    decimal AmountRecovered,
    DateOnly? LastContactDate,
    DateOnly? NextFollowUpDate,
    int ContactAttempts,
    string? ClosureReason,
    string? Notes);

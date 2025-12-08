namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionCases.Get.v1;

public sealed record CollectionCaseResponse(
    DefaultIdType Id,
    string CaseNumber,
    DefaultIdType LoanId,
    DefaultIdType MemberId,
    DefaultIdType? AssignedCollectorId,
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

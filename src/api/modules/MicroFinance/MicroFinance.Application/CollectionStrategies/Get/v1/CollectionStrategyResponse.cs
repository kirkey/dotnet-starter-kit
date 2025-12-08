namespace FSH.Starter.WebApi.MicroFinance.Application.CollectionStrategies.Get.v1;

public sealed record CollectionStrategyResponse(
    DefaultIdType Id,
    string Code,
    string Name,
    string? Description,
    DefaultIdType? LoanProductId,
    int TriggerDaysPastDue,
    int? MaxDaysPastDue,
    decimal? MinOutstandingAmount,
    decimal? MaxOutstandingAmount,
    string ActionType,
    string? MessageTemplate,
    int Priority,
    int? RepeatIntervalDays,
    int? MaxRepetitions,
    bool EscalateOnFailure,
    bool RequiresApproval,
    bool IsActive,
    DateOnly? EffectiveFrom,
    DateOnly? EffectiveTo);

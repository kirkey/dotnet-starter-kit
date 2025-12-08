namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Get.v1;

public sealed record AmlAlertResponse(
    DefaultIdType Id,
    string AlertCode,
    DefaultIdType? MemberId,
    DefaultIdType? TransactionId,
    string AlertType,
    string Severity,
    string Status,
    string TriggerRule,
    string Description,
    decimal? TransactionAmount,
    DateTime AlertedAt,
    DateTime? InvestigationStartedAt,
    DateTime? ResolvedAt,
    DefaultIdType? AssignedToId,
    string? ResolutionNotes,
    string? SarReference,
    DateOnly? SarFiledDate,
    bool RequiresReporting);

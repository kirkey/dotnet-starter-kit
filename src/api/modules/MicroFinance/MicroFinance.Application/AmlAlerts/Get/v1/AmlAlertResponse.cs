namespace FSH.Starter.WebApi.MicroFinance.Application.AmlAlerts.Get.v1;

public sealed record AmlAlertResponse(
    Guid Id,
    string AlertCode,
    Guid? MemberId,
    Guid? TransactionId,
    string AlertType,
    string Severity,
    string Status,
    string TriggerRule,
    string Description,
    decimal? TransactionAmount,
    DateTime AlertedAt,
    DateTime? InvestigationStartedAt,
    DateTime? ResolvedAt,
    Guid? AssignedToId,
    string? ResolutionNotes,
    string? SarReference,
    DateOnly? SarFiledDate,
    bool RequiresReporting);

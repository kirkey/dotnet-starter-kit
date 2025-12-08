namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Get.v1;

/// <summary>
/// Response containing risk alert details.
/// </summary>
public sealed record RiskAlertResponse(
    DefaultIdType Id,
    string AlertNumber,
    string Title,
    string? Description,
    string Severity,
    string Source,
    string Status,
    decimal? ThresholdValue,
    decimal? ActualValue,
    decimal? Variance,
    DateTime AlertedAt,
    DefaultIdType? AssignedToUserId,
    bool IsEscalated,
    int EscalationLevel,
    DateTime? DueDate,
    bool IsOverdue,
    string? Resolution);

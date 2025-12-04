using Ardalis.Specification;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Get.v1;

/// <summary>
/// Specification for getting a risk alert by ID.
/// </summary>
public sealed class RiskAlertByIdSpec : Specification<RiskAlert, RiskAlertResponse>
{
    public RiskAlertByIdSpec(Guid id)
    {
        Query.Where(r => r.Id == id);

        Query.Select(r => new RiskAlertResponse(
            r.Id,
            r.AlertNumber,
            r.Title,
            r.Description,
            r.Severity,
            r.Source,
            r.Status,
            r.ThresholdValue,
            r.ActualValue,
            r.Variance,
            r.AlertedAt,
            r.AssignedToUserId,
            r.IsEscalated,
            r.EscalationLevel,
            r.DueDate,
            r.IsOverdue,
            r.Resolution));
    }
}

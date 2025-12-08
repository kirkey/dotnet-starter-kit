using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Get.v1;

/// <summary>
/// Request to get a risk alert by ID.
/// </summary>
public sealed record GetRiskAlertRequest(DefaultIdType Id) : IRequest<RiskAlertResponse>;

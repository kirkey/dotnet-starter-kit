using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskAlerts.Create.v1;

/// <summary>
/// Command to create a new risk alert.
/// </summary>
public sealed record CreateRiskAlertCommand(
    string AlertNumber,
    string Title,
    string Severity,
    string Source,
    DefaultIdType? RiskCategoryId = null,
    DefaultIdType? RiskIndicatorId = null,
    string? Description = null,
    decimal? ThresholdValue = null,
    decimal? ActualValue = null,
    DefaultIdType? BranchId = null,
    DefaultIdType? LoanId = null,
    DefaultIdType? MemberId = null,
    DateTime? DueDate = null) : IRequest<CreateRiskAlertResponse>;

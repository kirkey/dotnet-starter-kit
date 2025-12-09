using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Update.v1;

/// <summary>
/// Command to update an existing risk category.
/// </summary>
public sealed record UpdateRiskCategoryCommand(
    DefaultIdType Id,
    string? Name = null,
    string? Description = null,
    string? DefaultSeverity = null,
    decimal? WeightFactor = null,
    decimal? AlertThreshold = null,
    bool? RequiresEscalation = null,
    int? EscalationHours = null,
    int? DisplayOrder = null,
    string? Notes = null)
    : IRequest<UpdateRiskCategoryResponse>;

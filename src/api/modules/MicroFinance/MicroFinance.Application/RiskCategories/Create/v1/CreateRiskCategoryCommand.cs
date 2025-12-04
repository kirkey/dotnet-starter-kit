using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Create.v1;

public sealed record CreateRiskCategoryCommand(
    string Code,
    string Name,
    string RiskType,
    string? Description = null,
    Guid? ParentCategoryId = null,
    string DefaultSeverity = "Medium",
    decimal WeightFactor = 1.0m,
    decimal? AlertThreshold = null,
    bool RequiresEscalation = false,
    int? EscalationHours = null,
    int DisplayOrder = 0) : IRequest<CreateRiskCategoryResponse>;

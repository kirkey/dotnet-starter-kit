namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Get.v1;

public sealed record RiskCategoryResponse(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    string RiskType,
    Guid? ParentCategoryId,
    string DefaultSeverity,
    decimal WeightFactor,
    decimal? AlertThreshold,
    bool RequiresEscalation,
    int? EscalationHours,
    int DisplayOrder,
    string Status);

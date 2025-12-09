namespace FSH.Starter.WebApi.MicroFinance.Application.RiskCategories.Update.v1;

/// <summary>
/// Response returned after successfully updating a risk category.
/// </summary>
/// <param name="Id">The unique identifier of the updated risk category.</param>
public sealed record UpdateRiskCategoryResponse(DefaultIdType Id);

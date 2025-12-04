namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Get.v1;

/// <summary>
/// Response containing collateral type details.
/// </summary>
public sealed record CollateralTypeResponse(
    Guid Id,
    string Name,
    string Code,
    string Category,
    string? Description,
    string Status,
    decimal DefaultLtvPercent,
    decimal MaxLtvPercent,
    int DefaultUsefulLifeYears,
    decimal AnnualDepreciationRate,
    bool RequiresInsurance,
    bool RequiresAppraisal);

using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Create.v1;

/// <summary>
/// Command to create a new collateral type.
/// </summary>
public sealed record CreateCollateralTypeCommand(
    string Name,
    string Code,
    string Category,
    decimal DefaultLtvPercent,
    decimal MaxLtvPercent,
    int DefaultUsefulLifeYears = 10,
    decimal AnnualDepreciationRate = 10,
    string? Description = null) : IRequest<CreateCollateralTypeResponse>;

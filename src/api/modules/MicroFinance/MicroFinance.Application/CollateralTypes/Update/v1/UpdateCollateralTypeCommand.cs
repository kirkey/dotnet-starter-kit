using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Update.v1;

/// <summary>
/// Command to update an existing collateral type.
/// </summary>
public sealed record UpdateCollateralTypeCommand(
    DefaultIdType Id,
    string? Name = null,
    string? Description = null,
    decimal? DefaultLtvPercent = null,
    decimal? MaxLtvPercent = null,
    int? DefaultUsefulLifeYears = null,
    decimal? AnnualDepreciationRate = null,
    bool? RequiresInsurance = null,
    bool? RequiresAppraisal = null,
    int? RevaluationFrequencyMonths = null,
    bool? RequiresRegistration = null,
    string? RequiredDocuments = null,
    string? Notes = null,
    int? DisplayOrder = null)
    : IRequest<UpdateCollateralTypeResponse>;

using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.Create.v1;

public sealed record CreateCollateralInsuranceCommand(
    DefaultIdType CollateralId,
    string PolicyNumber,
    string InsurerName,
    string InsuranceType,
    decimal CoverageAmount,
    decimal PremiumAmount,
    decimal Deductible,
    DateOnly EffectiveDate,
    DateOnly ExpiryDate,
    bool IsMfiAsBeneficiary = true) : IRequest<CreateCollateralInsuranceResponse>;

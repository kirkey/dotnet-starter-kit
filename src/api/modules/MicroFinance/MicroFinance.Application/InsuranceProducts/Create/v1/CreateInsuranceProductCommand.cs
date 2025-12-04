using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceProducts.Create.v1;

public sealed record CreateInsuranceProductCommand(
    [property: DefaultValue("INS-LP-001")] string Code,
    [property: DefaultValue("Loan Protection Insurance")] string Name,
    [property: DefaultValue("LoanProtection")] string InsuranceType,
    [property: DefaultValue(10000)] decimal MinCoverage,
    [property: DefaultValue(1000000)] decimal MaxCoverage,
    [property: DefaultValue("PercentOfLoan")] string PremiumCalculation,
    [property: DefaultValue(1.5)] decimal PremiumRate,
    [property: DefaultValue(null)] string? Description = null,
    [property: DefaultValue(null)] string? Provider = null,
    [property: DefaultValue(0)] int WaitingPeriodDays = 0,
    [property: DefaultValue(true)] bool PremiumUpfront = true,
    [property: DefaultValue(false)] bool MandatoryWithLoan = false,
    [property: DefaultValue(null)] int? MinAge = null,
    [property: DefaultValue(null)] int? MaxAge = null)
    : IRequest<CreateInsuranceProductResponse>;

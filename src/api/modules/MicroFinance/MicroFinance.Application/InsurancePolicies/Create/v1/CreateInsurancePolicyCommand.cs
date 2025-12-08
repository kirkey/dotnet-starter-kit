using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Create.v1;

/// <summary>
/// Command to create a new insurance policy for a member.
/// </summary>
public sealed record CreateInsurancePolicyCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType MemberId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType InsuranceProductId,
    [property: DefaultValue("2025-01-01")] DateOnly StartDate,
    [property: DefaultValue("2026-01-01")] DateOnly EndDate,
    [property: DefaultValue(1000000)] decimal CoverageAmount,
    [property: DefaultValue(50000)] decimal PremiumAmount,
    [property: DefaultValue(30)] int WaitingPeriodDays = 30,
    [property: DefaultValue(null)] DefaultIdType? LoanId = null,
    [property: DefaultValue(null)] string? BeneficiaryName = null,
    [property: DefaultValue(null)] string? BeneficiaryRelation = null
) : IRequest<CreateInsurancePolicyResponse>;

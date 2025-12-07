using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.RecordPremium.v1;

/// <summary>
/// Command to record a premium payment on an insurance policy.
/// </summary>
public sealed record RecordInsurancePolicyPremiumCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid Id,
    [property: DefaultValue(5000)] decimal Amount,
    [property: DefaultValue("2025-01-15")] DateOnly PaymentDate,
    [property: DefaultValue("REF-001")] string? ReferenceNumber = null
) : IRequest<RecordInsurancePolicyPremiumResponse>;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Create.v1;

/// <summary>
/// Response after creating an insurance policy.
/// </summary>
public sealed record CreateInsurancePolicyResponse(DefaultIdType Id, string PolicyNumber);

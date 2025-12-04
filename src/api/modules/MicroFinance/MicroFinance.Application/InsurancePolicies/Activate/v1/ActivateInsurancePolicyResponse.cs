namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Activate.v1;

/// <summary>
/// Response after activating an insurance policy.
/// </summary>
public sealed record ActivateInsurancePolicyResponse(Guid Id, string Status);

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Cancel.v1;

/// <summary>
/// Response after cancelling an insurance policy.
/// </summary>
public sealed record CancelInsurancePolicyResponse(Guid Id, string Status);

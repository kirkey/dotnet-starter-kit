using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Activate.v1;

/// <summary>
/// Command to activate an insurance policy after first premium payment.
/// </summary>
public sealed record ActivateInsurancePolicyCommand(DefaultIdType Id) : IRequest<ActivateInsurancePolicyResponse>;

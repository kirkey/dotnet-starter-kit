using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.Get.v1;

/// <summary>
/// Request to get an insurance policy by ID.
/// </summary>
public sealed record GetInsurancePolicyRequest(DefaultIdType Id) : IRequest<InsurancePolicyResponse>;

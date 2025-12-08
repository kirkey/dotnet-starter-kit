using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InsuranceClaims.Get.v1;

/// <summary>
/// Request to get an insurance claim by ID.
/// </summary>
public sealed record GetInsuranceClaimRequest(DefaultIdType Id) : IRequest<InsuranceClaimResponse>;

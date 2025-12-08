using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanOfficerTargets.Get.v1;

/// <summary>
/// Request to get a loan officer target by ID.
/// </summary>
public sealed record GetLoanOfficerTargetRequest(DefaultIdType Id) : IRequest<LoanOfficerTargetResponse>;

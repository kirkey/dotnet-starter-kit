using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanApplications.Get.v1;

/// <summary>
/// Request to get a loan application by ID.
/// </summary>
public sealed record GetLoanApplicationRequest(DefaultIdType Id) : IRequest<LoanApplicationResponse>;

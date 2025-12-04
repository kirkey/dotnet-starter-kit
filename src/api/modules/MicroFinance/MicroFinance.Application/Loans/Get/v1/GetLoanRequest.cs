using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.Loans.Get.v1;

public sealed record GetLoanRequest(Guid Id) : IRequest<LoanResponse>;

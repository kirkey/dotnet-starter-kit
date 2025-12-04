using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanRestructures.Get.v1;

public sealed record GetLoanRestructureRequest(Guid Id) : IRequest<LoanRestructureResponse>;

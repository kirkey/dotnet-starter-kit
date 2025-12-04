using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;

public sealed record GetLoanWriteOffRequest(Guid Id) : IRequest<LoanWriteOffResponse>;

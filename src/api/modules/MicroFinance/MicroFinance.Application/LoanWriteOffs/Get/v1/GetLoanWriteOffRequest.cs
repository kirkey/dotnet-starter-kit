using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanWriteOffs.Get.v1;

public sealed record GetLoanWriteOffRequest(DefaultIdType Id) : IRequest<LoanWriteOffResponse>;

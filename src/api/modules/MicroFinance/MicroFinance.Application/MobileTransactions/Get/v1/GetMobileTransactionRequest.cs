using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;

public sealed record GetMobileTransactionRequest(Guid Id) : IRequest<MobileTransactionResponse>;

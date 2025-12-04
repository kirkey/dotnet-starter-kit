using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Get.v1;

public sealed record GetShareTransactionRequest(Guid Id) : IRequest<ShareTransactionResponse>;

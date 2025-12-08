using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Get.v1;

public sealed record GetShareTransactionRequest(DefaultIdType Id) : IRequest<ShareTransactionResponse>;

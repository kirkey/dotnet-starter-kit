using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Get.v1;

public sealed record GetSavingsTransactionRequest(Guid Id) : IRequest<SavingsTransactionResponse>;

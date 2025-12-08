using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Fail.v1;

public sealed record FailMobileTransactionCommand(
    DefaultIdType Id,
    string FailureReason) : IRequest<FailMobileTransactionResponse>;

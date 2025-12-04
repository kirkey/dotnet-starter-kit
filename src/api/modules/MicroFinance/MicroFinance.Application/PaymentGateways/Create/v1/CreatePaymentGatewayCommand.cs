using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Create.v1;

public sealed record CreatePaymentGatewayCommand(
    string Name,
    string Provider,
    decimal TransactionFeePercent,
    decimal TransactionFeeFixed,
    decimal MinTransactionAmount,
    decimal MaxTransactionAmount) : IRequest<CreatePaymentGatewayResponse>;

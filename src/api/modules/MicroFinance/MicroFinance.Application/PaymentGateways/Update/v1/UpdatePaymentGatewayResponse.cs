namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Update.v1;

/// <summary>
/// Response after updating a payment gateway.
/// </summary>
public sealed record UpdatePaymentGatewayResponse(
    DefaultIdType Id,
    string Provider,
    string Status,
    decimal TransactionFeePercent,
    decimal TransactionFeeFixed,
    decimal MinTransactionAmount,
    decimal MaxTransactionAmount);

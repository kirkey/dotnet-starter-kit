using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Update.v1;

/// <summary>
/// Command to update a payment gateway configuration.
/// </summary>
public sealed record UpdatePaymentGatewayCommand(
    DefaultIdType Id,
    string? Name = null,
    decimal? TransactionFeePercent = null,
    decimal? TransactionFeeFixed = null,
    decimal? MinTransactionAmount = null,
    decimal? MaxTransactionAmount = null,
    bool? SupportsRefunds = null,
    bool? SupportsRecurring = null,
    bool? SupportsMobileWallet = null,
    bool? SupportsCardPayments = null,
    bool? SupportsBankTransfer = null,
    int? TimeoutSeconds = null,
    int? RetryAttempts = null) : IRequest<UpdatePaymentGatewayResponse>;

namespace FSH.Starter.WebApi.MicroFinance.Application.PaymentGateways.Deactivate.v1;

/// <summary>
/// Response after deactivating a payment gateway.
/// </summary>
/// <param name="Id">The unique identifier of the deactivated gateway.</param>
/// <param name="Provider">The payment provider name.</param>
/// <param name="Status">The new status of the gateway.</param>
public sealed record DeactivatePaymentGatewayResponse(
    DefaultIdType Id,
    string Provider,
    string Status);

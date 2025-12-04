namespace FSH.Starter.WebApi.MicroFinance.Application.InsurancePolicies.RecordPremium.v1;

/// <summary>
/// Response after recording a premium payment.
/// </summary>
public sealed record RecordPremiumPaymentResponse(Guid Id, decimal AmountPaid, decimal TotalPremiumPaid);

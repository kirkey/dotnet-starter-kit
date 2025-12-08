namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.RecordPayment.v1;

public sealed record RecordSettlementPaymentResponse(DefaultIdType Id, string Status, decimal AmountPaid, decimal RemainingBalance);

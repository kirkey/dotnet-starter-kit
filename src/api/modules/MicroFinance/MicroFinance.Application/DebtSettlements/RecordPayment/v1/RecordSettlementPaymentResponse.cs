namespace FSH.Starter.WebApi.MicroFinance.Application.DebtSettlements.RecordPayment.v1;

public sealed record RecordSettlementPaymentResponse(Guid Id, string Status, decimal AmountPaid, decimal RemainingBalance);

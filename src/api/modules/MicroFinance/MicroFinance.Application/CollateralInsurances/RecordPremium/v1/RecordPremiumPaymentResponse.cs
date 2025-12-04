namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.RecordPremium.v1;

public sealed record RecordPremiumPaymentResponse(Guid Id, DateOnly LastPremiumPaidDate, DateOnly NextPremiumDueDate);

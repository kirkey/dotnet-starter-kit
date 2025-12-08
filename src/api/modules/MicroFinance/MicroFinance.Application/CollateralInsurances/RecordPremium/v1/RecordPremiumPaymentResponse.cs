namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralInsurances.RecordPremium.v1;

public sealed record RecordCollateralInsurancePremiumResponse(DefaultIdType Id, DateOnly LastPremiumPaidDate, DateOnly NextPremiumDueDate);
